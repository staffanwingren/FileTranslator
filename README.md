FileTranslator
==============

Command line tool to convert lists of data between different formats.

Basic usage:
Run FileTranslator as a command line tool and supply the input file as well as the desired output file path

```
ft.exe -in .\input.csv -out .\output.json
```

List of possible arguments:

| Argument | Shortcut | Description | Example |
| -------- | -------- | ----------- | ------- |
| -in      |          | The input file path. Required! | -In C:\myinfile.ext |
| -out     |          | The output file path. If not supplied out.txt will be used. | -Out D:\myoutfile.ext |
| -encoding |         | The encoding to use. If encoding is not supplied UTF-8 is assumed. | -encoding utf8 |
| -input-encoding |    | The encoding to use when reading the input file. If not supplied UTF-8 is assumed. | -input-encoding utf8 |
| -output-encoding |   | The encoding to use when writing the output file. If not supplied UTF-8 is assumed. | -output-encoding utf8 |
| -parser |            | The parser to use when reading the input file. Parsers are file type readers. If not supplied the file name supplied in -in will be used to figure out what parser to use. | -parser Csv |
| -printer |           | The printer to use when writing the output file. Printers are file type writers. If not supplied the file name supplied in -out will be used to figure out what printer to use. | -printer Json |
| -model |             | The model that defines the data. If not supplied the program will try to generate a model based on -in and -parser parameters | -model KWSpecialReport |
