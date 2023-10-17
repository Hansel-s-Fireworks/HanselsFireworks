using UnityEngine;

public class MaterialController : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Material targetMaterial; // ��� ��Ƽ����

    // ��Ƽ���� �� ������ ������ �� ����� ������
    public Color newColor;
    public float newFloatValue;

    private void Start()
    {
        // SkinnedMeshRenderer�� �������� �ʾ��� ���, ���� ���� ������Ʈ���� ã�ƺ��ϴ�.
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
        // ��� ��Ƽ������ �����ɴϴ�.
        Material[] materials = skinnedMeshRenderer.materials;
        targetMaterial = materials[1];
        // ��� ��Ƽ������ ã�Ƽ� �����մϴ�.
        
                // ��Ƽ���� �� ������ �����մϴ�.
        // materials[1].SetColor("_Color", newColor); // "_Color"�� ��Ƽ���� ���� �ٸ� �� �ֽ��ϴ�.
        // materials[1].SetFloat("_FloatValue", newFloatValue); // "_FloatValue" ���� ��Ƽ���� ���� �ٸ� �� �ֽ��ϴ�.

                // ��Ƽ������ �ٽ� �Ҵ��մϴ�.
                // skinnedMeshRenderer.materials = materials;
           
    }
}
