#define __NO_STD_VECTOR // Use cl::vector instead of STL version

#include <stdio.h>
#include <time.h>

#include <fstream>
#include <iostream>
#include <string>
#include <iterator>

#ifdef usingCL

#include <cstdio>
#include <cstdlib>
#include <CL/opencl.h>


inline void checkErr(cl_int err, const char * name) {
  if (err != CL_SUCCESS) {
  std::cerr << "ERROR: " << name  << " (" << err << ")" << std::endl;
  exit(EXIT_FAILURE);
  }
}

#define WITH_MAIN

#ifdef WITH_MAIN
int main(int argc, const char * argv[]){
  const std::string hw("Hello World\n");

  cl_int err;
  cl::vector< cl::Platform > platformList;
  cl::Platform::get(&platformList);
  checkErr(platformList.size()!=0 ? CL_SUCCESS : -1, "cl::Platform::get");
  std::cerr << "Platform number is: " << platformList.size() << std::endl;std::string platformVendor;
  platformList[0].getInfo((cl_platform_info)CL_PLATFORM_VENDOR, &platformVendor);
  std::cerr << "Platform is by: " << platformVendor << "\n";
  cl_context_properties cprops[3] = {CL_CONTEXT_PLATFORM, (cl_context_properties)(platformList[0])(), 0};cl::Context context(CL_DEVICE_TYPE_CPU,cprops,NULL,NULL,&err);
  checkErr(err, "Context::Context()");


}
#endif

#endif
