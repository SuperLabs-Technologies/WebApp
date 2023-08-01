# Documentation
Welcome to the official documentation for WebApp!

## Table of Contents
* [Set up WebApp](#set-up-webapp)
* [Development](#development)
  * [Starting the test project](#starting-the-test-project)
  * [Setting up events](#setting-up-events)

## Set up WebApp
Setting up a WebApp project is very simple, we have created a script you can run to instantly set up and prepare your development environment.
Copy the script and paste it into your command prompt.
```bat
cd C:\
mkdir WebApp
cd WebApp
git init
git clone https://github.com/SuperLabs-Technologies/WebApp.git
start .
```

## Development

### Starting the test project
So you have created your WebApp project and wish to proceed, what do you do to enable a debugging-friendly environment?

1. Go to `index.html`
2. Navigate to `var releaseEnabled = true`
3. Set the variable to `false`

> Set this to `true` before releasing

Now open the solution in VS code. Start off by making sure all dependencies are referenced. If you launch the application and the window doesn't show, open nuget package manager, remove WebView2 and install the latest version.

On the right hand side, you'll find a folder named "App". This is the interface of the application.
![image](https://user-images.githubusercontent.com/57600814/207610990-d1605749-f073-4f39-81bc-e0fa601f0239.png)

To launch the app with the test project, hit the "Start" button using Debug mode.
![image](https://user-images.githubusercontent.com/57600814/207612876-9812fdc9-4ab2-4497-bc91-b66716010502.png)

> If the window is blank, reinstall WebView2 through the visual studio nuget manager.

### Setting up events
Events are pretty easy to set up. Lets use the test project as an example.

In the index.html file, we have sat up an event listener to listen to mouse clicks on the "Try me" button.
```js
document.querySelector(".webapp-button").addEventListener("click", function() {
    window.chrome.webview.postMessage(JSON.stringify({
      id: "buttonElement",
      eventName: "click",
      message: "Hello, from WebApp!"
    }));
});
```

And on the MainWindow.xaml.cs file, we have created a field that will look for the "buttonElement" element.
```cs
Element buttonElement = new Element("buttonElement");
```

And in the method where we are initializing webapp on the C# part, we have set up an event listener
```cs
buttonElement.AddEventListener("click", (EventArguments args) =>
{
    MessageBox.Show(args.args["message"]);
});
```
