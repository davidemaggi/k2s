# k2s
A KubeConfig manager for the lazy developer of today

## Wizard
The main k2s command, it will prompt the available Contexts and Namespaces to let you switch between them in a fast, lazy way...
oh, and it allows you to filter... in case you have a gazillion of contexts
```shell
k2s
```
### Arrow Keys
![](https://github.com/davidemaggi/k2s/blob/main/k2s.Tools/imgs/readme/gif_wizard1.gif?raw=true)

## Merge
>With great projects comes great clusters (cit. Uncle Ben).

This Command will merge the desired yaml file in your kubeconfig file
```shell
k2s merge your/file/path/config.yaml
```
![](https://github.com/davidemaggi/k2s/blob/main/k2s.Tools/imgs/readme/gif_merge.gif?raw=true)
## Alias
In my day by day job I need to access many clusters, and they share very similar names(thank you DevOps team ❤) , lets give them some fancy new ones.

```shell
k2s alias
```
![](https://github.com/davidemaggi/k2s/blob/main/k2s.Tools/imgs/readme/gif_alias.gif?raw=true)

## Delete
Bored of a context? Delete it! nobody will notice it

```shell
k2s delete
```
![](https://github.com/davidemaggi/k2s/blob/main/k2s.Tools/imgs/readme/gif_delete.gif?raw=true)

## Port Forwarding
A wizard based port forwarding configuration.

```shell
k2s forward
```
![](https://github.com/davidemaggi/k2s/blob/main/k2s.Tools/imgs/readme/gif_fwd.gif?raw=true)


## Info
Display the current Context and Namespace.
Or the raw yaml file if you are brave enough
```shell
k2s info
```
![](https://github.com/davidemaggi/k2s/blob/main/k2s.Tools/imgs/readme/gif_info1.gif?raw=true)

## Help
Display the help view, if you are too lazy to read the previous instructions...
In such case, we are the same... I like you...
```shell
k2s help
```
![](https://github.com/davidemaggi/k2s/blob/main/k2s.Tools/imgs/readme/gif_help.gif?raw=true)


## Global Flags

| Flag      | Alias | Optional | Description                                                                    | Default               |
|-----------|:-----:|:-------:|--------------------------------------------------------------------------------|-----------------------|
| --file  |  -f   |    x    | The config file to manage| $USERDIR/.kube/config |
| --force   |  -f   |    x    | Perform the operation forcing it even if no actual action is needed  🚧WIP🚧           | false                 |
| --verbose |  -v   |    x    | Display extended logs                                                          | false                 |
| --kubectl |  -k   |    x    | return a kubectl compliant command of the desired action                                                          | false                 |
## Install

k2s is making its first baby steps, It's available as standalone binaries for all major platforms that you can download and install by yourself, the only package manager from where you can download k2s is Chocolatey on windows.
>Why? because Ii mainly work on windows that's why
 
>⭐: Preferred Method
### macOS 🍎
#### Manual 🔨
1. Download the binary that fits your OS from [Release](https://github.com/davidemaggi/k2s/releases) page
2. Copy k2s executable file to an accessible directory on your Mac(e.g /usr/local/bin)
3. Add the Folder to your PATH Environment variable, if not already configured
4. Enjoy k2s

### Linux 🐧
#### Manual 🔨
1. Download the binary that fits your OS from [Release](https://github.com/davidemaggi/k2s/releases) page
2. Copy k2s executable file to an accessible directory on your PC(e.g /usr/bin)
3. Add the Folder to your PATH Environment variable, if not already configured
4. Enjoy k2s
5. 
### Windows 🪟
#### Manual 🔨
1. Download the binary that fits your OS from [Release](https://github.com/davidemaggi/k2s/releases) page
2. Copy k2s.exe file to an accessible directory on your PC(e.g C:\Users\<YourUserName>\AppData\Local)
3. Add the Folder to your PATH Environment variable, if not already configured
4. Enjoy k2s
#### Chocolatey 📦 ⭐
Simply execute the choco command to install it
```shell
choco install k2s
```
To update it
```shell
choco upgrade k2s
```