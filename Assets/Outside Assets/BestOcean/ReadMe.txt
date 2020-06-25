
This time, the sea surface is developed with priority. It mimics the realization of the sea surface in 2017 siggraph, and tries to restore the effect.
 At the same time, due to performance limitations, the complex parts are simplified as much as possible, and the effect is maximized.
Seawater has a number of parameters that can be controlled, including lighting, disturbances, scattering, caustics, etc., so you can add a very good sea experience to your game.




Quick Start
Please set the color space to linear space.
Drag Ocean in the Prefab directory to the scene. Note that due to lighting effects, I hope you can adjust the lighting information to be the same as in my demo scene. 
I will paste some settings into the document. Since you can't upload the project settings, I hope you can set the project to linear space yourself. 
The performance in gamma space will be a bit strange, the color is blue, you can adapt the gamma space by gamma-translinear function, 
but I strongly recommend implementing your project in linear space.
The height of the camera will affect the sea level, so it is recommended to adjust the camera to a suitable height.
For land in the scene, you need to drag the IslandDepthCache below it to get depth information. Since you don't know the size of the land, 
you need to set the appropriate size yourself. This setting will be described later in the script parameters.

Ocean shader parameter settings


_Normal Map - The normal map of the ocean. It also includes two settings for intensity and zoom.
Ocean base scattering setting - Contains base color, scattering color, and solar coefficient and attenuation coefficient
Height-based scatter setting - Since the intensity of the scattering changes with height, the scattering can be enhanced by the maximum height and height intensity settings, and the highly scattering color can be adjusted. 
Shoal scattering setting - For shoals, the scattering will be stronger than the center of the ocean, so the depth, depth and scattering color can enhance the performance of the shoal.
_Environmental reflection setting - The environmental reflection is measured by sampling the sky box and adjusting the effect by the Finney coefficient.
Direct light setting - Direct light is the Phong illumination model
Bubble setting - The foam is more complex, producing a white foam at the peak by calculating the degree of tearing of the seawater. At the same time, due to the obstruction of objects at the shore, it is also necessary to generate white foam according to the depth. Therefore, the parameters are divided into two parts, one is used to control the peak strength and color of the peak foam, and the other is to control the strength and disturbance of the shape of the foam on the shore.
Transparent setting- Transparent settings include transparency calculated from depth and disturbance strength
Caustics setting - Caustics calculates the intersection of the scene and the water surface and simulates underwater flicker and projection through a caustic map.



There are a lot of parameters under each setting. For some simple parameters, you can know his purpose by name, and some parameters are due to the result of mathematics. 
The relationship needs to look at a long paper, which is difficult to fully explain. However, because it seems to be the right principle, 
you can try different parameters to experience different effects. Good looking is correct.