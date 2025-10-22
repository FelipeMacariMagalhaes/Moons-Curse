using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class MenuPrincipalManager : MonoBehaviour
{
    [SerializeField] private string nomeDoLevelDeJogo;
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelOpcoes;
    

   public void jogar()
   {
        SceneManager.LoadScene(nomeDoLevelDeJogo);
   }

   public void AbrirOpcoes()
   {
        painelMenuInicial.SetActive(false);
        painelOpcoes.SetActive(true);

   }
   public void FecharOpcoes()
   {
        painelOpcoes.SetActive(false);
        painelMenuInicial.SetActive(true);
   }
   public void SairJogo()
   {
    Debug.Log("Sair do Jogo");
        Application.Quit();
   }
}
