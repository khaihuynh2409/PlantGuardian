using PlantGuardian.API.DTOs;

namespace PlantGuardian.API.Services
{
    public interface IBeanCareService
    {
        BeanCareProfileDto? GetCareProfile(string plantType);
        int GetDaysUntilNextWatering(string plantType, DateTime? lastWatered);
    }

    public class BeanCareService : IBeanCareService
    {
        private static readonly Dictionary<string, BeanCareProfileDto> _profiles = new(StringComparer.OrdinalIgnoreCase)
        {
            ["BlackBean"] = new BeanCareProfileDto
            {
                PlantType = "BlackBean",
                DisplayName = "Đậu Đen",
                Description = "Đậu đen (Vigna mungo) là cây họ đậu phổ biến, giàu protein và chất xơ. Phù hợp trồng ở Việt Nam.",
                WateringFrequencyDays = 2,
                SunlightHoursPerDay = 6,
                SoilPhRange = "6.0 - 6.8",
                IdealTemperatureCelsius = "20 - 30°C",
                GrowthDurationDays = 90,
                FertilizerSchedule = "Bón phân NPK lúc trồng, phân Urê lúc 20 ngày, phân kali lúc ra hoa.",
                CommonDiseases = new List<string>
                {
                    "Bệnh gỉ sắt (Rust) - lá có đốm nâu đỏ",
                    "Bệnh đốm lá (Leaf spot) - đốm đen trên lá",
                    "Sâu đục quả - làm hỏng hạt",
                    "Bệnh thối rễ nếu đất quá ẩm"
                },
                GrowthStages = new List<string>
                {
                    "Nảy mầm (0-7 ngày)",
                    "Cây con (7-21 ngày)",
                    "Phát triển lá (21-45 ngày)",
                    "Ra hoa (45-60 ngày)",
                    "Kết quả (60-80 ngày)",
                    "Thu hoạch (80-90 ngày)"
                },
                HarvestTips = "Thu hoạch khi quả chuyển màu đen và khô. Hái vào buổi sáng, phơi khô 2-3 ngày trước khi tách hạt.",
                WateringTips = "Tưới đều, tránh để đất quá khô hoặc quá ướt. Giai đoạn ra hoa cần nhiều nước nhất."
            },
            ["Soybean"] = new BeanCareProfileDto
            {
                PlantType = "Soybean",
                DisplayName = "Đậu Nành",
                Description = "Đậu nành (Glycine max) là cây công nghiệp quan trọng, nguồn protein thực vật cao nhất.",
                WateringFrequencyDays = 3,
                SunlightHoursPerDay = 8,
                SoilPhRange = "6.0 - 7.0",
                IdealTemperatureCelsius = "20 - 30°C",
                GrowthDurationDays = 120,
                FertilizerSchedule = "Đậu nành tự cố định đạm. Bón lân và kali lúc trồng. Tránh bón quá nhiều đạm.",
                CommonDiseases = new List<string>
                {
                    "Bệnh phấn trắng (Powdery mildew) - bột trắng trên lá",
                    "Nấm Phytophthora - thối thân gốc",
                    "Sâu ăn lá (Defoliators)",
                    "Tuyến trùng rễ (Soybean cyst nematode)"
                },
                GrowthStages = new List<string>
                {
                    "Nảy mầm (0-10 ngày)",
                    "Cây con (10-30 ngày)",
                    "Phát triển sinh dưỡng (30-60 ngày)",
                    "Ra hoa (60-80 ngày)",
                    "Hình thành quả (80-100 ngày)",
                    "Chín sinh lý (100-120 ngày)"
                },
                HarvestTips = "Thu hoạch khi 95% quả chuyển màu vàng nâu. Dùng máy hoặc hái tay, phơi nắng 3-5 ngày.",
                WateringTips = "Cần nhiều nước giai đoạn ra hoa và hình thành quả. Hạn chế tưới giai đoạn chín để hạt đều."
            },
            ["FavaBean"] = new BeanCareProfileDto
            {
                PlantType = "FavaBean",
                DisplayName = "Đậu Rộng",
                Description = "Đậu rộng (Vicia faba) còn gọi là đậu tằm, thích hợp khí hậu mát. Quả to và hạt lớn.",
                WateringFrequencyDays = 4,
                SunlightHoursPerDay = 6,
                SoilPhRange = "6.5 - 7.5",
                IdealTemperatureCelsius = "15 - 22°C",
                GrowthDurationDays = 150,
                FertilizerSchedule = "Bón phân hữu cơ trước khi trồng. Bón kali và lân lúc 30 ngày. Không cần bón nhiều đạm.",
                CommonDiseases = new List<string>
                {
                    "Bệnh rỉ sắt (Bean rust) - đốm gỉ sắt dưới lá",
                    "Bệnh chocolate spot - vết nâu chocolate",
                    "Rệp đen (Black bean aphid) - bầy rệp đen trên ngọn",
                    "Bệnh thối rễ Pythium ở đất ướt"
                },
                GrowthStages = new List<string>
                {
                    "Nảy mầm (0-14 ngày)",
                    "Cây con (14-40 ngày)",
                    "Phát triển thân lá (40-80 ngày)",
                    "Ra hoa (80-110 ngày)",
                    "Hình thành quả (110-130 ngày)",
                    "Thu hoạch (130-150 ngày)"
                },
                HarvestTips = "Thu hoạch khi quả căng và xanh đậm (ăn tươi) hoặc khi quả khô và đen (lấy hạt khô). Hái nhẹ tay.",
                WateringTips = "Chịu hạn tốt hơn đậu khác. Tưới sâu 1 lần mỗi 4 ngày. Tránh tưới vào buổi tối để hạn chế nấm."
            }
        };

        public BeanCareProfileDto? GetCareProfile(string plantType)
        {
            _profiles.TryGetValue(plantType, out var profile);
            return profile;
        }

        public int GetDaysUntilNextWatering(string plantType, DateTime? lastWatered)
        {
            if (!_profiles.TryGetValue(plantType, out var profile)) return -1;
            if (lastWatered == null) return 0;

            var nextWatering = lastWatered.Value.AddDays(profile.WateringFrequencyDays);
            var daysLeft = (nextWatering - DateTime.UtcNow).Days;
            return Math.Max(0, daysLeft);
        }
    }
}
