# AxelSmash

A clone of Scott Hanselman's [BabySmash](https://github.com/shanselman/babysmash) made to play with UWP & amuse my son (Axel)

## Design

Inputs provided by a baby are *smashes* (`IBabySmash`)

* Keyboard
* Controller
* Voice

The controller observe theses and produce one or more *giggle* (`IGiggle`)

* Speech
* Sound
* Picture

That are handled by their corresponding *giggle player*