import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DeviceTypeService {

  constructor() { }
  getDeviceType(): string {
    let filePath: string = '../../../assets/config/config_device_type.json';
    let result: any;
    var rawFile = new XMLHttpRequest();
    rawFile.open("GET", filePath, false);
    rawFile.onreadystatechange = function () {
      if (rawFile.readyState === 4) {
        if (rawFile.status === 200 || rawFile.status == 0) {
          let isIdemiaDevice = JSON.parse(rawFile.responseText).Idemia;
          let isUNVDevice = JSON.parse(rawFile.responseText).UNV;
          let isHANDevice = JSON.parse(rawFile.responseText).HAN;
          if (isIdemiaDevice) {
            result = 'idemiadevice'
          }
          if (isUNVDevice) {
            result = 'unvdevice'
          }
          if (isHANDevice) {
            result = 'handevice'
          }
        }
      }
    }
    rawFile.send(null);
    return result;
  }
}
