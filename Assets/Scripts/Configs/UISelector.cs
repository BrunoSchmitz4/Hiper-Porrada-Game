using UnityEngine;                 
using UnityEngine.EventSystems;     

public class UISelectorFollow : MonoBehaviour
{
    public RectTransform selector; //Referenciando o seletor (mãozinha)
    public Vector2 defaultOffset = new Vector2(-120f, 0f); //Posição do seletor

    [Header("Áudio")]
    public AudioSource audioSource;  
    public AudioClip moveClip;  

    
    string lastButtonName = ""; //Guarda apenas o nome do botão anterior

    void Update()
    {
        //Se não tiver botão selecionado, não retorna nada
        if (EventSystem.current.currentSelectedGameObject == null) return;

        //Pega o botão selecionado (agora)
        GameObject current = EventSystem.current.currentSelectedGameObject;

        //Se o próprio seletor for selecionada, ignora (ele estava indo pro lado quando selecionava o mesmo)
        if (current == selector.gameObject) return;

        //Pega a posição do botão na tela
        RectTransform target = current.GetComponent<RectTransform>();
        if (target == null) return; //Se não for um botão de UI, ele para

        //Usa a distância padrão
        Vector2 offset = defaultOffset;

        //Verifica se o botão tem uma distância personalizada
        SelectorOffset customOffset = current.GetComponent<SelectorOffset>();
        if (customOffset != null)
            offset = customOffset.offset;

        //Move o seletor para a posição do botão + distância
        selector.position = target.position + (Vector3)offset;

       
        //Verifica se mudou de botão comparando o nome do botão
        if (current.name != lastButtonName)
        {
            //Toca o som de troca de botão se estiver configurado
            if (audioSource != null && moveClip != null)
            {
                audioSource.PlayOneShot(moveClip);
            }
            
            //Atualiza o nome do último botão selecionado
            lastButtonName = current.name;
        }
    }
}