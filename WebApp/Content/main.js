/*
Copyright 2017 Google Inc.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

'use strict';

// This code is adapted from
// https://rawgit.com/Miguelao/demos/master/mediarecorder.html

var mediaSource = new MediaSource();
mediaSource.addEventListener('sourceopen', handleSourceOpen, false);
var mediaRecorder;
var recordedBlobs;
var sourceBuffer;

var gumVideo = document.querySelector('video#gum');
var recordedVideo = document.querySelector('video#recorded');

var recordButton = document.querySelector('button#record');
var playButton = document.querySelector('button#play');
var downloadButton = document.querySelector('button#download');
var rcdbuttonStart = document.querySelector("#imgRecordStart");
var rcdbuttonStop = document.querySelector("#imgRecordStop");


rcdbuttonStart.onclick = startRecordingNew;
rcdbuttonStop.onclick = stopRecordingNew;

recordButton.onclick = toggleRecording;
//playButton.onclick = play;
//downloadButton.onclick = download;

// window.isSecureContext could be used for Chrome
//var isSecureOrigin = location.protocol === 'https:' ||
// location.host.includes('localhost');
//if (!isSecureOrigin) {
//alert('getUserMedia() must be run from a secure origin: HTTPS or localhost.' +
// '\n\nChanging protocol to HTTPS');
//  location.protocol = 'HTTPS';
//}

var constraints = {
    audio: true,
    video: false
};

navigator.mediaDevices.getUserMedia(
    constraints
).then(
    successCallback,
    errorCallback
);

function successCallback(stream) {
    console.log('getUserMedia() got stream: ', stream);
    window.stream = stream;
    gumVideo.srcObject = stream;
}

function errorCallback(error) {
    console.log('navigator.getUserMedia error: ', error);
}

function handleSourceOpen(event) {
    console.log('MediaSource opened');
    sourceBuffer = mediaSource.addSourceBuffer('audio/mp3');
    console.log('Source buffer: ', sourceBuffer);
}

function handleDataAvailable(event) {
    if (event.data && event.data.size > 0) {
        recordedBlobs.push(event.data);
    }
}

function handleStop(event) {
    console.log('Recorder stopped: ', event);
    console.log('Recorded Blobs: ', recordedBlobs);
}

function toggleRecording() {
    if (recordButton.textContent === 'Start Recording') {
        startRecording();
    } else {
        stopRecording();
        recordButton.textContent = 'Start Recording';
        //playButton.disabled = false;
        //downloadButton.disabled = false;
    }
}
function startRecordingNew() {
    startRecording();
    $("#imgRecordStop").show();
    $("#imgRecordStart").hide();
    var fiveMinutes = 60 * 1,
        display = document.querySelector('#time');
    startTimer(fiveMinutes, display);
    $("#time").show();


}
function stopRecordingNew() {
    stopRecording();
    $("#imgRecordStop").hide();
    $("#imgRecordStart").show();
    $("#time").hide();
}

// The nested try blocks will be simplified when Chrome 47 moves to Stable
function startRecording() {
    var options = { mimeType: 'audio/webm' };
    recordedBlobs = [];
    try {
        mediaRecorder = new MediaRecorder(window.stream, options);
    } catch (e0) {
        console.log('Unable to create MediaRecorder with options Object: ', e0);
        try {
            options = { mimeType: 'audio/mp3' };
            mediaRecorder = new MediaRecorder(window.stream, options);
        } catch (e1) {
            console.log('Unable to create MediaRecorder with options Object: ', e1);
            try {
                options = 'audio/mp3'; // Chrome 47
                mediaRecorder = new MediaRecorder(window.stream, options);
            } catch (e2) {
                alert('MediaRecorder is not supported by this browser.\n\n' +
                    'Try Firefox 29 or later, or Chrome 47 or later, with Enable experimental Web Platform features enabled from chrome://flags.');
                console.error('Exception while creating MediaRecorder:', e2);
                return;
            }
        }
    }
    console.log('Created MediaRecorder', mediaRecorder, 'with options', options);
    recordButton.textContent = 'Stop Recording';
    //playButton.disabled = true;
    //downloadButton.disabled = true;
    mediaRecorder.onstop = handleStop;
    mediaRecorder.ondataavailable = handleDataAvailable;
    mediaRecorder.start(10); // collect 10ms of data
    console.log('MediaRecorder started', mediaRecorder);
}

function stopRecording() {
    mediaRecorder.stop();
    //recordedVideo.controls = true;
}

function play() {
    var type = (recordedBlobs[0] || {}).type;
    var superBuffer = new Blob(recordedBlobs, { type });
    recordedVideo.src = window.URL.createObjectURL(superBuffer);
}

function download() {
    var blob = new Blob(recordedBlobs, { type: 'audio/mp3' });
    alert(blob)
    var reader = new FileReader();
    reader.readAsDataURL(blob);
    reader.onloadend = function () {
        var base64data = reader.result;
        console.log(base64data);
    }
    var url = window.URL.createObjectURL(blob);
    var a = document.createElement('a');
    a.style.display = 'none';
    a.href = url;
    a.download = 'test.mp3';
    document.body.appendChild(a);
    a.click();
    setTimeout(function () {
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
    }, 100);
}

function startTimer(duration, display) {
    var start = Date.now(),
        diff,
        minutes,
        seconds;
    function timer() {
        // get the number of seconds that have elapsed since 
        // startTimer() was called
        diff = duration - (((Date.now() - start) / 1000) | 0);

        // does the same job as parseInt truncates the float
        minutes = (diff / 60) | 0;
        seconds = (diff % 60) | 0;

        minutes = minutes < 10 ? "0" + minutes : minutes;
        seconds = seconds < 10 ? "0" + seconds : seconds;

        display.textContent = minutes + ":" + seconds;

        if (diff <= 0) {
            // add one second so that the count down starts at the full duration           
            // example 05:00 not 04:59
            $("#time").hide();
            stopRecording();
            $("#imgRecordStart").show();
            $("#imgRecordStop").hide();
            start = Date.now() + 1000;
        }
    };
    // we don't want to wait a full second before the timer starts
    timer();
    setInterval(timer, 1000);
}
