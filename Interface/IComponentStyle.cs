
public interface IComponentStyle
{
    void AwakeInit();   // Awake 에서만 초기화
    void EnableInit();  // Enable 되었을 때만 초기화
    void StartInit();   // Start 에서만 초기화
    void ReInit();      // 코드 로직 중에 초기화 필요한 경우   
}
