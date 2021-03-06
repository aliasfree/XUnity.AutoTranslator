﻿# XUnity Auto Translator

## Text Frameworks
This is an auto translation mod that hooks into the unity game engine and attempts to provide translations for the following text frameworks for Unity:
 * UGUI
 * NGUI
 * TextMeshPro
 * IMGUI

It does go to the internet, in order to provide the translation, so if you are not comfortable with that, dont use it.
 
## Plugin Frameworks
The mod can be installed into the following Plugin Managers:
 * [BepInEx](https://github.com/bbepis/BepInEx)
 * [IPA](https://github.com/Eusth/IPA)
 * UnityInjector

Installations instructions for both methods can be found below.

Additionally it can be installed without a dependency on a plugin manager through ReiPatcher. However, this approach is not recommended if you use one of the above mentioned Plugin Managers!

## Configuration
The default configuration file, looks as such (2.6.0+):

```ini
[Service]
Endpoint=GoogleTranslate         ;Endpoint to use. Can be ["GoogleTranslate", "BaiduTranslate", "GoogleTranslateHack", "ExciteTranslate"]

[General]
Language=en                      ;The language to translate into
FromLanguage=ja                  ;The original language of the game

[Files]
Directory=Translation                                          ;Directory to search for cached translation files
OutputFile=Translation\_AutoGeneratedTranslations.{lang}.txt   ;File to insert generated translations into

[TextFrameworks]
EnableUGUI=True                  ;Enable or disable UGUI translation
EnableNGUI=True                  ;Enable or disable NGUI translation
EnableTextMeshPro=True           ;Enable or disable TextMeshProp translation
EnableIMGUI=False                ;Enable of disable IMGUI translation
AllowPluginHookOverride=True     ;Allow other text translation plugins to override this plugin's hooks

[Behaviour]
Delay=0                          ;Delay to wait before attempting to translate a text in seconds
MaxCharactersPerTranslation=150  ;Max characters per text to translate
IgnoreWhitespaceInDialogue=True  ;Whether or not to ignore whitespace, such as newlines, in dialogue keys
MinDialogueChars=20              ;The length of the text for it to be considered a dialogue
ForceSplitTextAfterCharacters=0  ;Split text into multiple lines once the translated text exceeds this number of characters
CopyToClipboard=False            ;Whether or not to copy hooked texts to clipboard
MaxClipboardCopyCharacters=450   ;Max number of characters to hook to clipboard at a time

[Baidu]
BaiduAppId=                      ;OPTIONAL, needed if BaiduTranslate is configured
BaiduAppSecret=                  ;OPTIONAL, needed if BaiduTranslate is configured

[Debug]
EnablePrintHierarchy=False       ;Used for debugging
EnableConsole=False              ;Enables the console. Do not enable if other plugins (managers) handles this
EnableLog=False                  ;Enables extra logging for debugging purposes

[Migrations]
Enable=True                      ;Used to enable automatic migrations of this configuration file
Tag=2.9.0                        ;Tag representing the last version this plugin was executed under. Do not edit
```

## Key Mapping
The following key inputs are mapped:
 * ALT + T: Alternate between translated and untranslated versions of all texts provided by this plugin.
 * ALT + D: Dump untranslated texts (if no endpoint is configured)
 * ALT + R: Reload translation files. Useful if you change the text files on the fly.

## Installation
The plugin can be installed in following ways:

### BepInEx Plugin
REQUIRES: [BepInEx plugin manager](https://github.com/bbepis/BepInEx) (follow its installation instructions first!). 

 1. Download XUnity.AutoTranslator-BepIn-{VERSION}.zip from [releases](../../releases).
 2. Extract directly into the game directory, such that the plugin dlls are placed in BepInEx folder.

The file structure should like like this:
```
{GameDirectory}/BepInEx/XUnity.AutoTranslator.Plugin.Core.dll
{GameDirectory}/BepInEx/XUnity.AutoTranslator.Plugin.Core.BepInEx.dll
{GameDirectory}/BepInEx/ExIni.dll
{GameDirectory}/BepInEx/Translation/AnyTranslationFile.txt (this files will be auto generated by plugin!)
```

### IPA Plugin
REQUIRES: [IPA plugin manager](https://github.com/Eusth/IPA) (follow its installation instructions first!).

 1. Download XUnity.AutoTranslator-IPA-{VERSION}.zip from [releases](../../releases).
 2. Extract directly into the game directory, such that the plugin dlls are placed in Plugins folder.

The file structure should like like this
```
{GameDirectory}/Plugins/XUnity.AutoTranslator.Plugin.Core.dll
{GameDirectory}/Plugins/XUnity.AutoTranslator.Plugin.Core.IPA.dll
{GameDirectory}/Plugins/0Harmony.dll
{GameDirectory}/Plugins/ExIni.dll
{GameDirectory}/Plugins/Translation/AnyTranslationFile.txt (this files will be auto generated by plugin!)
 ```

### UnityInjector Plugin
REQUIRES: UnityInjector (follow its installation instructions first!).

 1. Download XUnity.AutoTranslator-UnityInjector-{VERSION}.zip from [releases](../../releases).
 2. Extract directly into the game directory, such that the plugin dlls are placed in UnityInjector folder. **This may not be game root directory!**

The file structure should like like this
```
{GameDirectory}/UnityInjector/XUnity.AutoTranslator.Plugin.Core.dll
{GameDirectory}/UnityInjector/XUnity.AutoTranslator.Plugin.Core.UnityInjector.dll
{GameDirectory}/UnityInjector/0Harmony.dll
{GameDirectory}/UnityInjector/ExIni.dll
{GameDirectory}/UnityInjector/Translation/AnyTranslationFile.txt (this files will be auto generated by plugin!)
 ```
 
### Standalone Installation (ReiPatcher)
REQUIRES: Nothing, ReiPatcher is provided by this download.

 1. Download XUnity.AutoTranslator-ReiPatcher-{VERSION}.zip from [releases](../../releases).
 2. Extract directly into the game directory, such that "SetupReiPatcherAndAutoTranslator.exe" is placed alongside other exe files.
 3. Execute "SetupReiPatcherAndAutoTranslator.exe". This will setup up ReiPatcher correctly.
 4. Execute the shortcut {GameExeName}.lnk that was created besides existing executables. This will patch and launch the game.
 5. From now on you can launch the game from the {GameExeName}.exe instead.

The file structure should like like this
```
{GameDirectory}/ReiPatcher/Patches/XUnity.AutoTranslator.Patcher.dll
{GameDirectory}/ReiPatcher/ExIni.dll
{GameDirectory}/ReiPatcher/Mono.Cecil.dll
{GameDirectory}/ReiPatcher/Mono.Cecil.Inject.dll
{GameDirectory}/ReiPatcher/Mono.Cecil.Mdb.dll
{GameDirectory}/ReiPatcher/Mono.Cecil.Pdb.dll
{GameDirectory}/ReiPatcher/Mono.Cecil.Rocks.dll
{GameDirectory}/ReiPatcher/ReiPatcher.exe
{GameDirectory}/{GameExeName}_Data/Managed/ReiPatcher.exe
{GameDirectory}/{GameExeName}_Data/Managed/XUnity.AutoTranslator.Plugin.Core.dll
{GameDirectory}/{GameExeName}_Data/Managed/0Harmony.dll
{GameDirectory}/{GameExeName}_Data/Managed/ExIni.dll
{GameDirectory}/AutoTranslator/AnyTranslationFile.txt (this files will be auto generated by plugin!)
 ```

## Translating Mods
Often other mods UI are implemented through IMGUI. As you can see above, this is disabled by default. By changing the "EnableIMGUI" value to "True", it will start translating IMGUI as well, which likely means that other mods UI will be translated.

## Integrating with Auto Translator
I have implemented a system that allows other dedicated translation mods to integrate with XUnity AutoTranslator.

Basically, as a mod author, you are able to, if you cannot find a translation to a string, simply delegate it to this mod, and you can do it without taking any references to this plugin.

Here's how it works, and what is required:
 * You must implement a Component (MonoBehaviour for instance) that this plugin is able to locate by simply traversing all objects during startup.
 * On this component you must add an event for the text hooks you want to override from XUnity AutoTranslator. This is done on a per text framework basis. The signature of these events must be: Func<object, string, string>. The arguments are, in order: 
    1. The component that represents the text in the UI. (The one that probably has a property called 'text').
    2. The untranslated text
    3. This is the return value and will be the translated text IF an immediate translation took place. Otherwise it will simply be null.
 * The signature for each framework looks like:
    1. UGUI: public static event Func<object, string, string> OnUnableToTranslateUGUI
    2. TextMeshPro: public static event Func<object, string, string> OnUnableToTranslateTextMeshPro
    3. NGUI: public static event Func<object, string, string> OnUnableToTranslateNGUI
    3. IMGUI: public static event Func<object, string, string> OnUnableToTranslateIMGUI
 * Also, the events can be either instance based or static.
