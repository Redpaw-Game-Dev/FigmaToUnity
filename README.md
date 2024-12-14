## Introduction
FigmaToUnity is a tool for uploading and updating Figma images in Unity. Its purpose is to speed up the work of Unity developers with Figma by removing a bunch of annoying steps from loading or updating assets from Figma that need to be performed for the slightest change in an image. With FigmaToUnity, when you need to update images, you will no longer need to do these steps:
- Go to the Figma file.
- Export the image to your PC.
- Replace the old image with a new one in the Unity editor.

The only thing you need to do is select the images you want to update and click the update button.

## System Requirements
Unity 2021.3 or newer.

## Dependencies
[Newtonsoft JSON.NET](https://www.newtonsoft.com/json)

## Installation
1. Open Unity Package Manager (Window/Package Manager).
2. Open "+" -> "Add package from git URL...".
3. Enter this URL in the text field https://github.com/Redpaw-Game-Dev/FigmaToUnity.git and press "Add".

## Getting Started
1. Create config file from Assets -> Create -> LazyRedpaw -> FigmaToUnity and open it.
2. If you don't have a Figma access token, create one. There is [the guide](https://www.figma.com/developers/api#access-tokens).
   
   2.1. You can leave the default token settings unless you need otherwise.
3. Enter your token in "Figma Token" field.
4. Choose save path for files.
5. Create as many elements as you need and fill them with name and Figma element URL. (Explanation of URL copying in the "Documentation" section)
   
   5.1. Files naming is up to you. Name it what you would normally name an image in a project.
   
   ![Знімок екрана 2024-12-14 083909](https://github.com/user-attachments/assets/ef7dd082-ce21-4ebb-91f9-88e06e610300)

   
6. Press "Send Request" and wait for the download to complete.![Знімок екрана 2024-12-14 090724](https://github.com/user-attachments/assets/759222b8-dc26-41ba-8dda-dd5dce81e27b)



## Documentation
**Data types:**
- **NewImageData** type for downloading new images. It contains:
	- A new file name.
	- The URL of the element from Figma.
- **UpdateImageData** type for already downloaded images. Elements of this type will be created automatically when the request is processed. But you can create it manually if you need to. It contains:
	- A toggle to include or exclude this element from request.
	- A reference of the downloaded image.
	- The URL of the element from Figma.
	- A toggle to delete the image referenced by the item from the project after the item is removed from the data list.
 
**URL copying**
- To copy URL select the image you want and click the right mouse button to call the context menu, then select "Copy/Paste as" -> "Copy link to selection".![Знімок екрана 2024-12-14 081417](https://github.com/user-attachments/assets/7646631d-dcd3-4c79-9073-908d702510e0)


## Useful tips
- When you need to create and download new version of some image, use Figma component instances as a URL source. This way, you don't need to change the URL of config elements after creating a new variant of an image, just change the variant in the instance property. [Figma Components guide](https://help.figma.com/hc/en-us/articles/360038662654-Guide-to-components-in-Figma) ![Pasted image 20240730172310](https://github.com/user-attachments/assets/8c7ffbb7-3cf0-4157-88dc-4442b61a9f79)
