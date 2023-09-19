import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import {Component,  OnInit} from '@angular/core';
import {NgForm} from "@angular/forms";

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit {
  fileToUpload: File | undefined;
  email: string | undefined;
  alertIsVisible: boolean = false;

  constructor(private http: HttpClient) {
  }

  ngOnInit() {
  }

  uploadFile(files: any) {
    if (files.length === 0) {
      return;
    }

    this.fileToUpload = <File>files[0];
  }

  showAlert() {
    if (this.alertIsVisible) {
      return;
    }

    this.alertIsVisible = true;
    setTimeout(()=> this.alertIsVisible = false,2500)
  }

  onSubmit(uploadForm: NgForm) {
    if (!this.fileToUpload) {
      return;
    }
    
    const formData = new FormData();
    formData.append('file', this.fileToUpload, this.fileToUpload.name);
    formData.append('email', this.email ? this.email : '');
    this.http.post('https://reenbitherasymiva.azurewebsites.net/FileUploader/UploadFile', formData)
      .subscribe({
        next: () => {
          this.showAlert();
          uploadForm.resetForm();
          this.fileToUpload=undefined;
          },
        error: (err: HttpErrorResponse) => console.log(err)
      });
  }

  onChangeMail(mail: HTMLInputElement) {
    this.email = mail.value;
  }

}
