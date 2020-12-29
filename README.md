## pdfconv - Util for converting a pdf file to each jpeg image.
PDF 파일의 각 페이지를 jpeg로 변환하는 유틸

### Options
```
  -o, --outdir           (Default: .\) base directory to save image

  -f, --force            (Default: false) Overwrite image, if exists

  --help                 Display this help screen.

  --version              Display version information.

  target_pdf (pos. 0)    Required. PDF file name to convert
```

### Simple Usage
```
pdfconv.exe test.pdf -o DEST_FOLDER -f
```