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
3. Enter your token in "Figma Token field".
4. Choose save path for files.
5. Create as many elements as you need and fill them with name and Figma element URL. (Explanation of URL copying in the "Documentation" section)![Pasted image 20240730174044](https://github.com/user-attachments/assets/c4ea9fc9-789d-4fac-b519-57c22ec55e33)
6. Press "Send Request" and wait for the download to complete.![Pasted image 20240730174206](https://github.com/user-attachments/assets/6a5b13e2-cfa7-4e3f-a1a1-d731d57f6c27)

## Documentation
**Data types:**
- **NewImageData** type for downloading new images. It contains:
	- A new file name.
	- The URL of the element from Figma.
- **UpdateImageData** type for already downloaded images. Elements of this type will be created automatically when the request is processed. But you can create it manually if you need to. It contains:
	- A reference of the downloaded image.
	- A toggle to include or exclude this element from request.
	- The URL of the element from Figma.
	- The "x" button to delete an image from the project.
	- The "-" button to remove an item from the list.
 
**URL copying**
- In order to copy URL select the image you want and click "Share" at the top of the properties section, then click "Copy link" at the top of the window that appears.
- When you copy the URL of a frame or simple element that is a child of another frame, you copy the URL of the main parent frame. For example, if you try to get the URL of Frame 4, you get the URL of Frame 2.![Pasted image 20240730164345](https://github.com/user-attachments/assets/8cd6324e-8415-42ff-875f-a999c4bc3bee)

- If you want to copy the URL of a simple element without a parent, you must place it in a frame or section. Otherwise, you will copy the URL of the entire Figma file.![Pasted image 20240730165140](https://github.com/user-attachments/assets/f5632668-cf79-41dd-a367-7e461b3d56b2)

- Copying the URL of a section that is a child of another section will work. For example, if you try to get Section 4, you will get it.![Pasted image 20240730164630](https://github.com/user-attachments/assets/6788ba41-2860-4883-a822-27137b56073d)


## Useful tips
- When you need to create and download new version of some image, use Figma component instances as a URL source. This way, you don't need to change the URL of config elements after creating a new variant of an image, just change the variant in the instance property.![Pasted image 20240730172310](https://github.com/user-attachments/assets/8c7ffbb7-3cf0-4157-88dc-4442b61a9f79)
