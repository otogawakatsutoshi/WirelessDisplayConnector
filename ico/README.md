# image icon

This image get from [irustya](https://www.irasutoya.com/).


windowsでiconを作成するいい感じの方法がないので、
wslのlinux経由でimagemagicを使うことにより、pngをiconに変換する。


```bash
convert group_family_asia.png -define icon:auto-resize=128,48,32,16 icon.ico
```

https://www.regentechlog.com/2014/04/03/imagemagick-multisize-ico/

## 画像の確認。

identify icon.ico 
