function playAudio(audioId) {
    var audio = document.getElementById(audioId);
    if (audio.paused) {
        stopAllPlayingSounds();
        var img = document.querySelector(`[attr-audio-id="${audioId}"]`);
        img.src = "/img/icons/pause.png";
        if (audio) {
            audio.play();
            audio.addEventListener("ended", function () {
                img.src = "/img/icons/play.png";
            });
        }
    } else {
        PauseAudio(audioId);
    }
}

function stopAllPlayingSounds() {
    document.querySelectorAll('audio').forEach(function (audio) {
        PauseAudio(audio.id);
    });
}

function PauseAudio(audioId) {
    var audio = document.getElementById(audioId);
    var img = document.querySelector(`[attr-audio-id="${audio.id}"]`);
    img.src = "/img/icons/play.png";
    audio.pause();
    audio.currentTime = 0;
}

