import {HttpClient, json} from 'aurelia-fetch-client';
import { Applicant } from 'applicant';


let httpClient = new HttpClient()
.configure(x => {
  x.withBaseUrl('https://localhost:44378/');
});

export class WebAPI {
  
  isRequesting = false;

  createApplicant(applicant: Applicant){
    this.isRequesting = true;
    return httpClient.fetch('api/applicants/create',
    {
      method: 'post',
      body: json(applicant)
    })
    .then(response => {
      this.isRequesting = false;
      return response.json();
    })
    .catch(err => {
      console.error(err);
    });
  }

  updateApplicant(applicant: Applicant){
    this.isRequesting = true;

    return httpClient.fetch('api/applicants/update',
    {
      method: 'put',
      body: json(applicant)
    })
    .then(response => {
      this.isRequesting = false;
      return response.json();
    })
    .catch(err => {
      console.log('An error occurred. ', err);
    });
  }

  getApplicants(){
    this.isRequesting = true;
    return httpClient.get('api/applicants/getall')
    .then(response => {
      this.isRequesting = false;
      return response.json();
    })
    .catch(err => {
      console.log('An error occurred. ', err);
    });
  }

  getApplicant(id){
    this.isRequesting = true;
    let url = `api/applicants/get/${id}`
    
    return httpClient.get(url)
      .then(response =>{
        this.isRequesting = false;
        return response.json();
      });
  }

  deleteApplicant(id){
    this.isRequesting = true;
    let url = `api/applicants/delete/${id}`
    
    return httpClient.delete(url)
      .then(response =>{
        this.isRequesting = false;
        return response.json();
      });
  }
}
