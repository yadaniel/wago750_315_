# using go outside of the GOPATH  

mkdir project_dir  
cd project_dir  

touch main.go  
vim main.go  

go mod init main  
go build  
./main  

go mod vendor  
creates vendor folder with all sources  

