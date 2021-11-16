var unityInstance = UnityLoader.instantiate("unityContainer", "%UNITY_WEBGL_BUILD_URL%", {onProgress: UnityProgress});

function initiateGame(){
    var unityInstance = UnityLoader.instantiate("unityContainer", "%UNITY_WEBGL_BUILD_URL%", {onProgress: UnityProgress});
}

window.initiateGame = initiateGame;
