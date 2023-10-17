using UnityEngine;

public class MaterialController : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Material targetMaterial; // 대상 머티리얼

    // 머티리얼 내 변수를 조정할 때 사용할 변수들
    public Color newColor;
    public float newFloatValue;

    private void Start()
    {
        // SkinnedMeshRenderer가 지정되지 않았을 경우, 현재 게임 오브젝트에서 찾아봅니다.
        if (skinnedMeshRenderer == null)
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AdjustMaterialProperties();
        }
    }

    public void AdjustMaterialProperties()
    {
        // 대상 머티리얼을 가져옵니다.
        Material[] materials = skinnedMeshRenderer.materials;
        targetMaterial = materials[1];
        // 대상 머티리얼을 찾아서 조정합니다.
        
                // 머티리얼 내 변수를 조정합니다.
        // materials[1].SetColor("_Color", newColor); // "_Color"는 머티리얼에 따라 다를 수 있습니다.
        // materials[1].SetFloat("_FloatValue", newFloatValue); // "_FloatValue" 역시 머티리얼에 따라 다를 수 있습니다.

                // 머티리얼을 다시 할당합니다.
                // skinnedMeshRenderer.materials = materials;
           
    }
}
