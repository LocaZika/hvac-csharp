using Mapster;

namespace hvac_backend.config;

public class MapsterConfig {
  public static void Register() {
    // Cấu hình mặc định toàn cục: bỏ qua null khi mapping
    TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);
    // Cấu hình chi tiết các dto -> entity
  }
}
