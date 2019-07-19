CC = gcc
CFLAGS = -Wall -std=c11
LFLAGS = 

SRC_FILES = main.c crc.c wago.c
OBJ_FILES = $(patsubst %.c,%.o,$(SRC_FILES))

.PHONY: clean

%.o: %.c
	$(CC) -c $< $(CFLAGS) -o $@

prog: $(OBJ_FILES)
	$(CC) -o main $(LFLAGS) $(OBJ_FILES)

clean:
	rm -f $(OBJ_FILES) main.exe

