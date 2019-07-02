# CS = /cygdrive/C/Windows/Microsoft.NET/Framework64/v4.0.30319/csc.exe
CS = "/cygdrive/C/Program Files (x86)/MSBuild/14.0/Bin/csc.exe"
MCS = "/cygdrive/C/Program Files (x86)/Mono/bin/mcs"

SRC = main.cs wago750.cs

.PHONY: clean

all: mono win

win: $(SRC)
	@# $(CS) -langversion:6 wago750.cs main.cs /target:exe /out:main.exe
	@$(CS) wago750.cs main.cs /target:exe /out:main.exe

mono: $(SRC)
	$(MCS) main.cs wago750.cs /out:main_mono.exe

clean:
	rm -f main.exe main_mono.exe
    
