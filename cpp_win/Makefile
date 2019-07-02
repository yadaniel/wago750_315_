GCC = g++
CLANG = clang++

# override from command line
# CXXFLAGS ?= -Wall
# in addition append flags
# CXXFLAGS += -Wunused -Wshadow
CXXFLAGS += -Wshadow

dirs := $(shell pwd)
srcfiles := $(shell find $(dirs) -type f -iname "*.cpp" -exec basename {} ";")
objfiles := $(patsubst %.cpp, %.o, $(srcfiles))
depfiles := $(patsubst %.cpp, %.d, $(srcfiles))

srcs := $(wildcard *.cpp)
objs := $(srcs:.cpp=.o)
deps := $(srcs:.cpp=.d)

.phony: all clean

all: $(objs)
	@echo $(objs)
	@echo linking
	$(GCC) -o main_gcc.exe main_gcc.o wago750_gcc.o
	$(CLANG) -o main_clang.exe main_clang.o wago750_clang.o

%.o: %.cpp
	@echo "compiling" $<
	@# just compile using GCC, so .o is created from .cpp and make will use timestamp for this file
	$(GCC) -c -o $(patsubst %.cpp,%_gcc.o,$<) $< $(CXXFLAGS) -std=c++17
	$(CLANG) -c -o $(patsubst %.cpp,%_clang.o,$<) $< $(CXXFLAGS) -std=c++17
	# $(CLANG) -c -o $(patsubst %.cpp,%_clang.o,$<) $< $(CXXFLAGS) -stdlib=libc++ -std=c++17

clean:
	rm -f main_*.o wago750_*.o main_*.exe *.orig

