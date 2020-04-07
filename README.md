


![](Media/XPL-Title.jpg)
<p>
<a href="https://github.com/QianMo/X-PostProcessing-Library/blob/master/LICENSE">
    <img alt="GitHub license" src ="https://img.shields.io/github/license/QianMo/X-PostProcessing-Library" />
</a>
<a href="https://github.com/QianMo/X-PostProcessing-Library/issues">
    <img alt="GitHub issues" src="https://img.shields.io/github/issues/QianMo/X-PostProcessing-Library">
</a>
<a href="https://github.com/QianMo/X-PostProcessing-Library/pulls">
    <img alt="GitHub pull requests" src ="https://img.shields.io/github/issues-pr/QianMo/X-PostProcessing-Library" />
</a>

</p>

X-PostProcessing Libray，简称XPL，是针对Unity引擎的高品质开源后处理库，旨在提供业界主流的高品质后处理特效的完整解决方案。目前已完美支持Unity Post-processing Stack v2，后续也将提供对Unity引擎URP/LWRP/HDRP的兼容支持。

**X-PostProcessing Library (XPL)** is a high quality post processing library for for Unity Post Processing Stack v2/LWRP/URP/HDRP




<div align=center><img src="https://github.com/QianMo/X-PostProcessing-Library/blob/master/Media/title-rendering.jpg"/> </div>



# 内容 Content

- Blur Effects
    - [Gaussian Blur](Assets/X-PostProcessing/Effects/GaussianBlur)
    - [Box Blur](Assets/X-PostProcessing/Effects/BoxBlur)
    - [Tent Blur](Assets/X-PostProcessing/Effects/TentBlur)
    - [Kawase Blur](Assets/X-PostProcessing/Effects/KawaseBlur)
    - [Dual Kawase Blur](Assets/X-PostProcessing/Effects/DualKawaseBlur)
    - [Dual Gaussian Blur](Assets/X-PostProcessing/Effects/DualGaussianBlur)
    - [Dual Box Blur](Assets/X-PostProcessing/Effects/DualBoxBlur)
    - [Dual Tent Blur](Assets/X-PostProcessing/Effects/DualTentBlur)
    - [Bokeh Blur](Assets/X-PostProcessing/Effects/BokehBlur)
    - [Tilt Shift Blur](Assets/X-PostProcessing/Effects/TiltShiftBlur)
    - [Tilt Shift Blur V2](Assets/X-PostProcessing/Effects/TiltShiftBlurV2)
    - [Iris Blur](Assets/X-PostProcessing/Effects/IrisBlur)
    - [Iris Blur V2](Assets/X-PostProcessing/Effects/IrisBlurV2)
    - [Grainy Blur](Assets/X-PostProcessing/Effects/GrainyBlur)
    - [Radial Blur](Assets/X-PostProcessing/Effects/RadialBlur)
    - [Radial Blur V2](Assets/X-PostProcessing/Effects/RadialBlurV2)
    - [Directional Blur](Assets/X-PostProcessing/Effects/DirectionalBlur)
- Color Adjustment Effects
- Pixelate Effects
- Glitch Effects
- Stylized Effects
- Painting Effects
- Image Processing Effects
- ...

More effects and LWRP/URP/HDRP version will arrive soon.



# 安装 Installation

有两种主要的安装X-PostProcessing Library的方法：

- 【方法一】 克隆或下载此Repo，并直接使用Unity打开。建议可先从已设置好后处理的示例场景 [Assets/Example/ExampleScene.unity](Assets/Example/ExampleScene.unity) 开始。

- 【方法二】 将[X-PostProcessing](Assets/X-PostProcessing) 文件夹放置在项目Assets路径下的任一位置，并确保Post Processing Stack v2也位于项目中。

You have two main ways to install X-PostProcessing Library :

- **[Method 1]**   Clone or download this repository , open with Unity Engine Editor and enjoy. It is recommended to start with the example scene [Assets/Example/ExampleScene.unity](Assets/Example/ExampleScene.unity) .
- **[Method 2]**  Place the [X-PostProcessing](Assets/X-PostProcessing) folder anywhere in your project, make sure that Post Processing Stack v2 is in the project as well, and enjoy.



# 使用 Usage


post processing profile 有各种不同的修改和添加方式，最常规的方法是，选中一个post processing profile ，在Inspetor窗口下：

- `Add effect... > X-PostProcessing > 选择一种新的后处理`

The new effect should be available for a post processing profile with different injection points，just like:

- `Add effect... > X-PostProcessing > Choose an effect`



# 环境 Environment
- 建议使用Unity 2017.2+。

- 如果使用的是Unity的旧版本（5.6和2017.1），则需要将[此文件夹](https://github.com/QianMo/X-PostProcessing-Library/tree/master/Assets/PostProcessing-2) 替换为 [pps v2 2.1.8](https://github.com/Unity-Technologies/PostProcessing/tree/bec8546fc498db388cedadd14021cc7006338cc4)。

- Unity 2017.2+  is recommended.

- if you use older versions of Unity (5.6 and 2017.1) , you need to replace [this folder](https://github.com/QianMo/X-PostProcessing-Library/tree/master/Assets/PostProcessing-2) with [pps v2 2.1.8](https://github.com/Unity-Technologies/PostProcessing/tree/bec8546fc498db388cedadd14021cc7006338cc4).


# 文章 Blog Post

- [高品质后处理：十种图像模糊算法的总结与实现](https://zhuanlan.zhihu.com/p/125744132) | [GitHub Version](https://github.com/QianMo/Game-Programmer-Study-Notes/blob/master/Content/%E9%AB%98%E5%93%81%E8%B4%A8%E5%90%8E%E5%A4%84%E7%90%86%EF%BC%9A%E5%8D%81%E7%A7%8D%E5%9B%BE%E5%83%8F%E6%A8%A1%E7%B3%8A%E7%AE%97%E6%B3%95%E7%9A%84%E6%80%BB%E7%BB%93%E4%B8%8E%E5%AE%9E%E7%8E%B0/README.md)

 
