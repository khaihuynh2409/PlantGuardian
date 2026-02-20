import os
import cv2
import numpy as np
import uvicorn
from fastapi import FastAPI, UploadFile, File, HTTPException
from dotenv import load_dotenv
from pydantic import BaseModel

load_dotenv()

app = FastAPI(title="PlantGuardian AI Service", version="2.0.0")

# ─── Bean knowledge base ────────────────────────────────────────────────────────
BEAN_KNOWLEDGE = {
    "BlackBean": {
        "name": "Đậu Đen (Black Bean / Vigna mungo)",
        "watering": "Tưới 2 ngày/lần. Giữ ẩm đều, không để đất khô nứt. Giai đoạn ra hoa cần nhiều nước nhất.",
        "sunlight": "6 giờ ánh sáng trực tiếp mỗi ngày.",
        "diseases": ["Gỉ sắt (đốm nâu đỏ dưới lá)", "Đốm lá (đốm đen trên lá)", "Sâu đục quả"],
        "harvest": "Thu hoạch khi quả chuyển đen và khô, khoảng 80-90 ngày sau trồng.",
        "soil_ph": "6.0 - 6.8",
        "tips": "Bón phân NPK lúc trồng, phân Urê lúc 20 ngày, kali lúc ra hoa."
    },
    "Soybean": {
        "name": "Đậu Nành (Soybean / Glycine max)",
        "watering": "Tưới 3 ngày/lần. Cần nhiều nước giai đoạn ra hoa và tạo quả. Hạn chế tưới khi quả chín.",
        "sunlight": "8 giờ ánh sáng đầy đủ mỗi ngày.",
        "diseases": ["Phấn trắng (bột trắng trên lá)", "Nấm Phytophthora (thối thân gốc)", "Rệp đậu tương"],
        "harvest": "Thu hoạch khi 95% quả vàng nâu, khoảng 100-120 ngày sau trồng.",
        "soil_ph": "6.0 - 7.0",
        "tips": "Đậu nành tự cố định đạm. Không bón quá nhiều đạm. Bón lân và kali là chính."
    },
    "FavaBean": {
        "name": "Đậu Rộng / Đậu Tằm (Fava Bean / Vicia faba)",
        "watering": "Tưới 4 ngày/lần. Chịu hạn tốt. Tưới sâu, tránh tưới tối để hạn chế nấm.",
        "sunlight": "6 giờ ánh sáng mỗi ngày. Thích khí hậu mát 15-22°C.",
        "diseases": ["Rỉ sắt Bean rust", "Chocolate spot (vết nâu)", "Rệp đen (Black bean aphid)"],
        "harvest": "Thu hoạch quả xanh (ăn tươi) hoặc quả khô đen (lấy hạt), khoảng 130-150 ngày.",
        "soil_ph": "6.5 - 7.5",
        "tips": "Bón phân hữu cơ trước trồng. Không cần bón nhiều đạm. Phát triển tốt ở khí hậu mát."
    }
}

# ─── Health check ────────────────────────────────────────────────────────────────

@app.get("/")
def read_root():
    return {"status": "online", "service": "PlantGuardian AI", "version": "2.0.0"}

@app.get("/health")
def health_check():
    return {"status": "healthy"}

# ─── Image Analysis ──────────────────────────────────────────────────────────────

@app.post("/analyze")
async def analyze_plant(file: UploadFile = File(...)):
    """Phân tích ảnh cây trồng chung: độ ẩm đất và sức khỏe dựa trên màu xanh."""
    try:
        contents = await file.read()
        nparr = np.frombuffer(contents, np.uint8)
        img = cv2.imdecode(nparr, cv2.IMREAD_COLOR)

        if img is None:
             raise HTTPException(status_code=400, detail="Invalid image data")

        hsv = cv2.cvtColor(img, cv2.COLOR_BGR2HSV)
        h, s, v = cv2.split(hsv)
        avg_brightness = np.mean(v)

        moisture_status = "Moderate"
        if avg_brightness < 80:
            moisture_status = "Wet"
        elif avg_brightness > 140:
            moisture_status = "Dry"

        lower_green = np.array([35, 40, 40])
        upper_green = np.array([85, 255, 255])
        mask = cv2.inRange(hsv, lower_green, upper_green)
        green_ratio = np.count_nonzero(mask) / (img.shape[0] * img.shape[1])

        health_status = "Healthy"
        if green_ratio < 0.1:
            health_status = "Poor (Low Greenery)"
        elif green_ratio > 0.3:
            health_status = "Thriving"

        return {
            "filename": file.filename,
            "analysis": {
                "soil_moisture": moisture_status,
                "health_assessment": health_status,
                "metrics": {
                    "avg_brightness": float(avg_brightness),
                    "green_ratio": float(green_ratio)
                },
                "suggestion": f"Soil seems {moisture_status}. Plant looks {health_status}."
            }
        }

    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))


@app.post("/analyze-bean")
async def analyze_bean(file: UploadFile = File(...), plant_type: str = "BlackBean"):
    """
    Phân tích hình ảnh đậu chuyên biệt.
    - Phát hiện màu lá vàng (thiếu N2), nâu (bệnh nấm)
    - Ước tính giai đoạn phát triển từ tỷ lệ xanh lá
    - Đánh giá nguy cơ bệnh
    plant_type: BlackBean | Soybean | FavaBean
    """
    try:
        contents = await file.read()
        nparr = np.frombuffer(contents, np.uint8)
        img = cv2.imdecode(nparr, cv2.IMREAD_COLOR)

        if img is None:
            raise HTTPException(status_code=400, detail="Invalid image data")

        hsv = cv2.cvtColor(img, cv2.COLOR_BGR2HSV)

        # 1. Green ratio (healthy foliage)
        lower_green = np.array([35, 40, 40])
        upper_green = np.array([85, 255, 255])
        green_mask = cv2.inRange(hsv, lower_green, upper_green)
        green_ratio = np.count_nonzero(green_mask) / (img.shape[0] * img.shape[1])

        # 2. Yellow ratio (nitrogen deficiency or over-ripening)
        lower_yellow = np.array([20, 80, 80])
        upper_yellow = np.array([35, 255, 255])
        yellow_mask = cv2.inRange(hsv, lower_yellow, upper_yellow)
        yellow_ratio = np.count_nonzero(yellow_mask) / (img.shape[0] * img.shape[1])

        # 3. Brown ratio (fungal disease / leaf spot)
        lower_brown = np.array([5, 50, 50])
        upper_brown = np.array([20, 200, 180])
        brown_mask = cv2.inRange(hsv, lower_brown, upper_brown)
        brown_ratio = np.count_nonzero(brown_mask) / (img.shape[0] * img.shape[1])

        # --- Diagnose ---
        health_score = 5
        issues = []
        suggestions = []

        if yellow_ratio > 0.15:
            health_score -= 2
            issues.append("Lá vàng nhiều - có thể thiếu đạm (N) hoặc sắt (Fe)")
            suggestions.append("Bổ sung phân Urê hoặc phân hữu cơ. Kiểm tra pH đất.")

        if brown_ratio > 0.10:
            health_score -= 2
            issues.append("Xuất hiện đốm nâu - nguy cơ bệnh nấm hoặc đốm lá")
            suggestions.append("Phun thuốc diệt nấm gốc đồng. Tránh tưới vào buổi tối.")

        if green_ratio < 0.1:
            health_score -= 1
            issues.append("Ít lá xanh - cây phát triển chậm hoặc thiếu ánh sáng")
            suggestions.append("Đảm bảo đủ 6-8 giờ ánh sáng mỗi ngày.")

        health_score = max(1, health_score)

        # --- Growth stage estimate ---
        growth_stage_estimate = "Không xác định"
        if green_ratio > 0.4:
            growth_stage_estimate = "Phát triển sinh dưỡng (Vegetative) - lá rậm"
        elif green_ratio > 0.2:
            growth_stage_estimate = "Cây con - đang phát triển tốt"
        elif green_ratio < 0.05 and yellow_ratio > 0.1:
            growth_stage_estimate = "Gần thu hoạch hoặc cây đang tàn"

        # Get bean-specific context
        bean_info = BEAN_KNOWLEDGE.get(plant_type, {})

        return {
            "filename": file.filename,
            "plant_type": plant_type,
            "analysis": {
                "health_score": health_score,
                "health_label": ["Rất kém", "Kém", "Trung bình", "Tốt", "Rất tốt"][health_score - 1],
                "growth_stage_estimate": growth_stage_estimate,
                "issues_detected": issues if issues else ["Không phát hiện bệnh rõ ràng"],
                "suggestions": suggestions if suggestions else ["Cây trông khỏe mạnh! Tiếp tục duy trì chế độ chăm sóc."],
                "disease_risk": "Cao" if brown_ratio > 0.1 else ("Trung bình" if brown_ratio > 0.05 else "Thấp"),
                "metrics": {
                    "green_ratio": round(float(green_ratio), 3),
                    "yellow_ratio": round(float(yellow_ratio), 3),
                    "brown_ratio": round(float(brown_ratio), 3)
                },
                "bean_care_reminders": {
                    "watering": bean_info.get("watering", ""),
                    "common_diseases": bean_info.get("diseases", [])
                }
            }
        }

    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))


# ─── Chat (Gemini AI) ────────────────────────────────────────────────────────────

import google.generativeai as genai

GENAI_API_KEY = os.getenv("GEMINI_API_KEY")
if GENAI_API_KEY:
    genai.configure(api_key=GENAI_API_KEY)
    model = genai.GenerativeModel('gemini-pro')
else:
    model = None
    print("Warning: GEMINI_API_KEY not found in environment variables.")


class ChatRequest(BaseModel):
    message: str
    context: str = ""

class BeanAdviceRequest(BaseModel):
    plant_type: str  # BlackBean | Soybean | FavaBean
    question: str
    growth_stage: str = ""  # Optional current growth stage


@app.post("/chat")
async def chat_with_plant_assistant(request: ChatRequest):
    """Chat tổng quát với trợ lý PlantGuardian."""
    if not model:
        raise HTTPException(status_code=503, detail="AI Service not configured with API Key.")

    try:
        SYSTEM_PROMPT = """Bạn là trợ lý chăm sóc cây trồng 'PlantGuardian' thông minh.
        Bạn am hiểu sâu về các loại cây, đặc biệt là: đậu đen (Black Bean), đậu nành (Soybean), đậu rộng/đậu tằm (Fava Bean), 
        cũng như xương rồng (Cactus), cây mọng nước (Succulent).
        Sử dụng thông tin được cung cấp (thời tiết, loại cây, trạng thái đất) để đưa ra lời khuyên cụ thể.
        Trả lời ngắn gọn, thân thiện, bằng tiếng Việt trừ khi được yêu cầu khác."""

        full_prompt = f"{SYSTEM_PROMPT}\n\nBối cảnh: {request.context}\n\nNgười dùng: {request.message}\nTrợ lý:"

        response = model.generate_content(full_prompt)
        return {"response": response.text}
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))


@app.post("/bean-advice")
async def get_bean_advice(request: BeanAdviceRequest):
    """
    Tư vấn chuyên biệt cho từng loại đậu với hoặc không có Gemini.
    Nếu không có API key, trả về lời khuyên từ knowledge base cục bộ.
    """
    bean_info = BEAN_KNOWLEDGE.get(request.plant_type)
    if not bean_info:
        raise HTTPException(
            status_code=400,
            detail=f"Loại đậu không hỗ trợ: {request.plant_type}. Hợp lệ: BlackBean, Soybean, FavaBean"
        )

    # Fallback knowledge base (no Gemini needed)
    fallback_advice = {
        "tưới": bean_info["watering"],
        "bệnh": "Các bệnh phổ biến: " + ", ".join(bean_info["diseases"]),
        "thu hoạch": bean_info["harvest"],
        "phân bón": bean_info["tips"],
        "ánh sáng": bean_info["sunlight"],
        "ph": f"pH đất lý tưởng: {bean_info['soil_ph']}"
    }

    # Check if any keyword matches for quick answer
    question_lower = request.question.lower()
    for keyword, answer in fallback_advice.items():
        if keyword in question_lower:
            if not model:
                return {
                    "plant_type": request.plant_type,
                    "plant_name": bean_info["name"],
                    "question": request.question,
                    "advice": answer,
                    "source": "local_knowledge_base"
                }

    # Use Gemini for detailed advice if available
    if not model:
        general_info = (
            f"Thông tin {bean_info['name']}:\n"
            f"- Tưới nước: {bean_info['watering']}\n"
            f"- Ánh sáng: {bean_info['sunlight']}\n"
            f"- Bệnh phổ biến: {', '.join(bean_info['diseases'])}\n"
            f"- Thu hoạch: {bean_info['harvest']}\n"
            f"- Phân bón: {bean_info['tips']}"
        )
        return {
            "plant_type": request.plant_type,
            "plant_name": bean_info["name"],
            "question": request.question,
            "advice": general_info,
            "source": "local_knowledge_base"
        }

    try:
        stage_context = f"Giai đoạn hiện tại: {request.growth_stage}." if request.growth_stage else ""
        BEAN_PROMPT = f"""Bạn là chuyên gia nông nghiệp về cây đậu. 
        Người dùng đang trồng {bean_info['name']}.
        {stage_context}
        
        Thông tin tham khảo:
        - Tưới nước: {bean_info['watering']}
        - Ánh sáng: {bean_info['sunlight']}
        - pH đất: {bean_info['soil_ph']}
        - Bệnh phổ biến: {', '.join(bean_info['diseases'])}
        - Thu hoạch: {bean_info['harvest']}
        - Phân bón: {bean_info['tips']}
        
        Câu hỏi: {request.question}
        
        Trả lời ngắn gọn và thực tế bằng tiếng Việt:"""

        response = model.generate_content(BEAN_PROMPT)
        return {
            "plant_type": request.plant_type,
            "plant_name": bean_info["name"],
            "question": request.question,
            "advice": response.text,
            "source": "gemini_ai"
        }
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))


if __name__ == "__main__":
    uvicorn.run("main:app", host="0.0.0.0", port=8000, reload=True)
