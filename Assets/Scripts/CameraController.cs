using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float baseFollowSpeed = 10f;  // Базовая скорость следования камеры
    public float horizontalOffsetFactor = 0.5f;  // Множитель для ограничения горизонтального смещения камеры
    public float verticalThreshold = 2f;  // Порог высоты для вертикального смещения камеры
    public float maxHorizontalOffset = 3f;  // Максимальное горизонтальное смещение камеры от центра
    public Vector3 defaultPosition;  // Дефолтная позиция камеры

    public float fixedHeightIncrement = 5f;  // Фиксированное значение подъема камеры
    public float heightLevel1 = 2f;  // Высота для уровня 1
    public float heightLevel2 = 4f;  // Высота для уровня 2

    private Vector3 offset;
    private PlayerController playerController;
    private float currentHeightLevel = 0f;  // Текущий уровень высоты камеры

    void Start()
    {
        offset = transform.position - target.position;
        defaultPosition = transform.position;  // Устанавливаем начальную позицию камеры как дефолтную
        playerController = target.GetComponent<PlayerController>();
    }

    void LateUpdate()
    {
        if (playerController == null) return;

        // Рассчитываем динамическую скорость следования камеры на основе скорости игрока
        float dynamicFollowSpeed = baseFollowSpeed + playerController.forwardSpeed;

        Vector3 desiredPosition = target.position + offset;

        // Ограничиваем горизонтальное смещение
        float horizontalOffset = Mathf.Clamp(target.position.x * horizontalOffsetFactor, -maxHorizontalOffset, maxHorizontalOffset);
        desiredPosition.x = defaultPosition.x + horizontalOffset;

        // Логика подъема камеры по уровням
        if (target.position.y > heightLevel2 && currentHeightLevel < heightLevel2)
        {
            currentHeightLevel = heightLevel2;
        }
        else if (target.position.y > heightLevel1 && currentHeightLevel < heightLevel1)
        {
            currentHeightLevel = heightLevel1;
        }
        else if (target.position.y <= heightLevel1)
        {
            currentHeightLevel = 0f;  // Возвращаемся на дефолтный уровень
        }

        // Устанавливаем вертикальную позицию камеры в зависимости от текущего уровня высоты
        desiredPosition.y = defaultPosition.y + currentHeightLevel * fixedHeightIncrement;

        // Плавное перемещение камеры
        transform.position = Vector3.Lerp(transform.position, desiredPosition, dynamicFollowSpeed * Time.deltaTime);
    }
}
