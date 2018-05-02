using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public enum UIState{ Game_Build, Game_Wave, Game_Pause, Game_Lose, Game_Win, MainMenu, MainMenu_Settings }
	private UIState uiState;
	private UIState lastUIState;

	public GameObject buildUI;
	public GameObject waveUI;
	public GameObject pauseMenuUI;
	public GameObject gameLoseUI;
	public GameObject gameWinUI;
	public GameObject mainMenuUI;
	public GameObject mainMenuSettingsUI;

	public Text qualityLevelText;
	public Text masterVolumeText;

	private void Start(){
		Time.timeScale = 1;
	}

	private void Update(){
		if(qualityLevelText != null)
			qualityLevelText.text = QualitySettings.names [QualitySettings.GetQualityLevel ()];
		if(masterVolumeText != null)
			masterVolumeText.text = (int)(AudioListener.volume * 100) + "";
	}

	public void ChangeUIState(int state){
		lastUIState = uiState;
		uiState = (UIState)state;

		if(uiState == UIState.Game_Build){
			SwitchUI (buildUI);
		}else if(uiState == UIState.Game_Wave){
			SwitchUI (waveUI);
		}else if(uiState == UIState.Game_Pause){
			SwitchUI (pauseMenuUI);
		}else if(uiState == UIState.Game_Lose){
			SwitchUI (gameLoseUI);
		}else if(uiState == UIState.Game_Win){
			SwitchUI (gameWinUI);
		}else if(uiState == UIState.MainMenu){
			SwitchUI (mainMenuUI);
		}else if(uiState == UIState.MainMenu_Settings){
			SwitchUI (mainMenuSettingsUI);
		}
	}

	public void GoToLastUIState(){
		ChangeUIState ((int)lastUIState);
	}

	public void LoadScene(string sceneName){
		SceneManager.LoadScene (sceneName);
	}

	public void QuitGame(){
		Application.Quit ();
	}

	private void SwitchUI(GameObject uiToShow){
		if(buildUI != null)
			buildUI.SetActive (false);
		if(waveUI != null)
			waveUI.SetActive (false);
		if(pauseMenuUI != null)
			pauseMenuUI.SetActive (false);
		if(gameLoseUI != null)
			gameLoseUI.SetActive (false);
		if(gameWinUI != null)
			gameWinUI.SetActive (false);
		if(mainMenuUI != null)
			mainMenuUI.SetActive (false);
		if(mainMenuSettingsUI != null)
			mainMenuSettingsUI.SetActive (false);

		if (uiToShow != null)
			uiToShow.SetActive (true);
	}

	public void IncreaseGraphicQuality(){
		QualitySettings.IncreaseLevel ();
	}

	public void DecreaseGraphicQuality(){
		QualitySettings.DecreaseLevel ();
	}

	public void RaiseMasterVolume(){
		AudioListener.volume = RoundToNearest10((int)(AudioListener.volume * 100) + 10) / 100f;
		AudioListener.volume = Mathf.Clamp01 (AudioListener.volume);
	}

	public void LowerMasterVolume(){
		AudioListener.volume = RoundToNearest10((int)(AudioListener.volume * 100) - 10) / 100f;
		AudioListener.volume = Mathf.Clamp01 (AudioListener.volume);
	}

	private int RoundToNearest10(int num){
		if (num % 10 == 0)
			return num;

		return (10 - num % 10) + num;
	}

}
