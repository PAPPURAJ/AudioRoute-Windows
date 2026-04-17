#define AppName "AudioRoute"
#ifndef AppVersion
  #define AppVersion "1.0.10"
#endif
#ifndef PublishDir
  #define PublishDir "..\dist\portable\win-x64"
#endif

[Setup]
AppId={{7A30654A-D53C-4F67-B7A6-A0DD78A11881}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher=AudioRoute
DefaultDirName={autopf}\{#AppName}
DefaultGroupName={#AppName}
DisableProgramGroupPage=yes
OutputDir=..\dist\installer
OutputBaseFilename=AudioRoute-Setup-{#AppVersion}
Compression=lzma
SolidCompression=yes
WizardStyle=modern
ArchitecturesInstallIn64BitMode=x64compatible

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "{#PublishDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\{#AppName}"; Filename: "{app}\AudioRoute.exe"
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\AudioRoute.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; Flags: unchecked

[Run]
Filename: "{app}\AudioRoute.exe"; Description: "Launch AudioRoute"; Flags: nowait postinstall skipifsilent
