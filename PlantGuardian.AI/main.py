import os
import cv2
import numpy as np
import uvicorn
from fastapi import FastAPI, UploadFile, File, HTTPException
from dotenv import load_dotenv

load_dotenv()

app = FastAPI(title="PlantGuardian AI Service", version="1.0.0")

@app.get("/")
def read_root():
    return {"status": "online", "service": "PlantGuardian AI"}

@app.get("/health")
def health_check():
    return {"status": "healthy"}

@app.post("/analyze")
async def analyze_plant(file: UploadFile = File(...)):
    try:
        contents = await file.read()
        nparr = np.frombuffer(contents, np.uint8)
        img = cv2.imdecode(nparr, cv2.IMREAD_COLOR)

        if img is None:
             raise HTTPException(status_code=400, detail="Invalid image data")

        # Basic Image Analysis Logic (Placeholder for advanced AI)
        # 1. Calculate Average Brightness to estimate Soil Moisture (Darker = Wetter)
        hsv = cv2.cvtColor(img, cv2.COLOR_BGR2HSV)
        h, s, v = cv2.split(hsv)
        avg_brightness = np.mean(v)
        
        # Heuristic: < 80 is likely wet/dark soil, > 150 is dry
        moisture_status = "Moderate"
        if avg_brightness < 80:
            moisture_status = "Wet"
        elif avg_brightness > 140:
            moisture_status = "Dry"

        # 2. Plant Health (Greenery check) - Ratio of Green Pixels
        # Green range in HSV: (35, 40, 40) to (85, 255, 255)
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

import google.generativeai as genai

# Configure Gemini
GENAI_API_KEY = os.getenv("GEMINI_API_KEY")
if GENAI_API_KEY:
    genai.configure(api_key=GENAI_API_KEY)
    model = genai.GenerativeModel('gemini-pro')
else:
    model = None
    print("Warning: GEMINI_API_KEY not found in environment variables.")

from pydantic import BaseModel

class ChatRequest(BaseModel):
    message: str
    context: str = ""

@app.post("/chat")
async def chat_with_plant_assistant(request: ChatRequest):
    if not model:
        raise HTTPException(status_code=503, detail="AI Service not configured with API Key.")
    
    try:
        # Construct prompt with context (e.g., plant info, weather)
        SYSTEM_PROMPT = """You are a helpful, expert plant care assistant known as 'PlantGuardian'. 
        You specialize in succulents, cacti, and carnivorous plants. 
        Use the provided context (weather, plant type, soil status) to give specific advice.
        Keep answers concise and friendly."""
        
        full_prompt = f"{SYSTEM_PROMPT}\n\nContext: {request.context}\n\nUser: {request.message}\nAssistant:"
        
        response = model.generate_content(full_prompt)
        return {"response": response.text}
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))

if __name__ == "__main__":
    uvicorn.run("main:app", host="0.0.0.0", port=8000, reload=True)
