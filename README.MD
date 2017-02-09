# How to run application for back-end development

  make sure you have already installed .net core ,then change to directory of build, run command
  `
  ./start.sh
  `
# Run application for consumption

  * make sure you have already installed docker   

  * run the scripts of `build\start.sh` 
  `
  cd ../build
  ./start.sh
  `
   
  * the application will start now ,you can open it on you web browser   
    `http://127.0.0.1:5000

# Rebuild docker image

  * run the script `build\build_webapi.sh` 
  `
  cd ../build
  ./build_webapi.sh
  `
   
  * the application will start now ,you can open it on you web browser   
    `http://127.0.0.1:5000