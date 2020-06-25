---Intro---

Thank you for purchasing the Candles and Candlesticks package. I hope you enjoy the assets included, and find a good use for them!

These assets have been tested in Unity 4 with the open-source Lux PBR shader and a custom-made Shader Forge shader. The textures are built for use with metalness shaders, and should also work with Marmoset Skyshop and Unity 5 with little modification.



---Models---

All models are included in OBJ format.

The candles are approximately 1000 polygons (trigons) each.

The wooden candlestick is approximately 2,700 tris; the brass "chamberstick" is approximately 3000, and the silver candlestick is 804.



---Shaders and Materials---

The custom shader has been included, and should be editable with Shader Forge, if you own Shader Forge and wish to use it to edit those shaders.

For the sake of simplifying the package, the total Lux shader framework has not been included (though the Lux materials and .shader files, as well as the environment cubemaps, are included).

Loading the entire Lux package into a Unity project can take a long time, so for the sake of not forcing every user who buys Candles and Candlesticks to go through this loading process, I have elected to make those files optional. If you wish to use the excellent Lux shaders, you can freely download the framework for incorporation into your Unity project from its creator's github at:
https://github.com/larsbertram69/Lux

You can also find more information about Lux in its discussion thread on the Unity forums:
http://forum.unity3d.com/threads/lux-an-open-source-physically-based-shading-framework.235027/



---Texture Maps---

Maps are Base (Diffuse), Normal, Roughness, Metalness, and Ambient Occlusion. A number of these maps (the ones that can get away with being grayscale) have been packed into individual channels of a single RGB texture. These are the textures with the _MASR.tga suffix, standing for Metalness (Red), Ambient (Green), Specular (Blue), and Roughness (Alpha).

The Lux shaders use the Specular channel, which was not originally used in developing these assets and can be considered a non-standard input for a Metalness workflow. A channel using a flat value has been included in the textures for those shaders. The Shader Forge shaders simply ignore the Specular channel.

The candles additionally use a grayscale Transmission (Thickness) map, allowing their shader to transmit a certain amount of scattered light in the manner of an actual wax candle.



---Contact Info---

If you have an issue with these models, please contact me at laith.i.a@gmail.com.



---Release Info---

Version 1.0 - Initial release on the Asset Store.
