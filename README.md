# ConwaysGameOfLife3D

Video footage:

[![image_012_1336](https://user-images.githubusercontent.com/43807989/119130724-db976880-ba38-11eb-9b8a-ecab49c33ed1.png)](https://img.youtube.com/vi/YOUTUBE_VIDEO_ID_HERE/0.jpg)

## Why did I do this?
A project created inspired by the requirements of "Computer Modeling" college course. 
I was supposed to implement Conway's Game of Life in an environment of my choice. 
I got inspired by the initial build of my algorithm presented on a 2D plane, so I decided to develop it further.

This project was created to train Unity development, and also to create a cool way to show an equally cool concept - the Conway's Game of Life.
Also, I didn't find anything like that in the internet and decided to do it on my own to upload later!

## How does it work
I tried to optimize the presentation as much as I could, managing a way to show each step of the algorithm on a plane in 3D space.
I create a finite amount of cubes - enough to populate a number of levels specified in the code. 
After the algorithm reaches that number of steps, I recycle the bottom level - I transport it to the top and change the layout according to the algorithm.
This way I don't instantiate objects at runtime and increase performance.
Alsop I tried to optimize the code in some ways - this may be trivial, but helped the simulation a lot.
