/*
it is necessary to use this concept because browser caching 
does not allow playing the correct audio in some cases
*/

var lastAudioSrc = "";
var audio = new Audio();
audio.addEventListener("ended", function () {
    document.querySelectorAll(`[attr-audio-id]`).forEach(i => i.src = "/img/icons/play.png");
    lastAudioSrc = "";
});
function playAudio(audioId) {
    document.querySelectorAll(`[attr-audio-id]`).forEach(i => i.src = "/img/icons/play.png");
    audio.src = audioId;
    if (lastAudioSrc != audioId) {
        audio.play();
        var img = document.querySelector(`[attr-audio-id="${audioId}"]`);
        img.src = "/img/icons/pause.png";
        lastAudioSrc = audioId;
    } else {
        lastAudioSrc = "";
        audio.pause();
    }
}

