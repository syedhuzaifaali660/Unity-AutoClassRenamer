# Auto Class Renamer for Unity

A lightweight, open-source Unity Editor tool that automatically keeps your C# class names in sync with their file names whenever you rename a script in the Project window.

Say goodbye to the classic Unity annoyance: renaming `PlayerController.cs`, only to find the internal `class PlayerController` didn't update — leaving you unable to attach the script to new GameObjects and causing confusion across your codebase.

## ✨ Features

- **Zero Clicks Required:** Works completely in the background. Just rename your `.cs` file in the Unity Editor, and the script handles the rest.
- **Smart Reference Updating:** Not only does it rename the main class, but it also scans your entire project and updates any typed references to that class in all other scripts — preventing real compilation errors from `GetComponent<OldName>()` and `public OldName myField;` calls.
- **Editor-Only:** Uses a custom Assembly Definition (`.asmdef`) to ensure this tool stays in the Editor and never bloats your final game build.
- **Safe Parsing:** Uses Regex to ensure it only targets actual class declarations, ignoring comments or unrelated string matches.

## 📦 Installation (Unity Package Manager)

This tool is formatted as a standard Unity Package. You can install it directly from this GitHub repository using the Unity Package Manager.

1. Open your Unity project.
2. Go to **Window > Package Manager**.
3. Click the **"+"** drop-down in the top-left corner.
4. Select **"Add package from git URL..."**
5. Paste the following link and click **Add**:
https://github.com/syedhuzaifaali660/Unity-AutoClassRenamer.git

## 🚀 How to Use

You don't need to open any windows or click any buttons.

1. Select a C# script in your Unity Project window.
2. Rename the file (e.g., from `Apple.cs` to `Orange.cs`).
3. The tool will automatically parse the file, change `class Apple` to `class Orange`, update any other scripts that referenced `Apple`, and tell Unity to recompile.

Check your Unity Console for a green success log confirming the changes!

## 🛠️ Requirements

* Unity 2021.3 or higher (Should work on older versions, but tested for modern UPM support).

## 🤝 Contributing

Feel free to open issues or submit pull requests if you want to add features or improve the regex parsing!

## 📄 License

MIT License. Free to use in personal and commercial projects.
