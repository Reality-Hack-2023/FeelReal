import matplotlib as plt
import numpy as np
import os
import librosa
import librosa.display

from fastai import *                                 
from fastai.vision.all import *
from fastai.vision.data import ImageDataLoaders
from fastai.tabular.all import *
from fastai.text.all import *
from fastai.vision.widgets import *

from fastapi import FastAPI
app = FastAPI()

@app.get("/ser")
def ser():
    # Load pickle file
    model = load_learner('./speech_02.pkl')

    # # Load in audio file and trim blank space
    y, sr = librosa.load('../FeelReal/Assets/StreamingAssets/test.wav')

    # yt, _ = librosa.effects.trim(y)

    # # Converting the sound clips into a melspectogram with librosa
    # # A mel spectrogram is a spectrogram where the frequencies are converted to the mel scale
    audio_spectogram = librosa.feature.melspectrogram(
        y=y, sr=sr, n_fft=1024, hop_length=100)

    # # Convert a power spectrogram (amplitude squared) to decibel (dB) units with power_to_db
    audio_spectogram = librosa.power_to_db(audio_spectogram, ref=np.max)

    librosa.display.specshow(audio_spectogram, y_axis='mel', fmax=20000, x_axis='time')

    p = os.path.join('../out.jpg')
    plt.savefig(p)

    # # Get model prediction
    e, _, _ = model.predict("../out.jpg")

    return e.upper()