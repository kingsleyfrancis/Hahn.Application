import { Router } from 'aurelia-router';
import { inject } from 'aurelia-dependency-injection';
import { WebAPI } from './web-api';
import { Applicant } from 'applicant';
import { EventAggregator } from 'aurelia-event-aggregator';
import {ApplicantCreated} from './messages';

@inject(WebAPI, Router)
export class CreateApplicant {
  applicant: Applicant = new Applicant();

  constructor(private api: WebAPI, private ea: EventAggregator, private router: Router){}

  newApplicant() {
    console.log('Create Applicant called: ', this.applicant);

    this.api.createApplicant(this.applicant).then(response => {
      console.log('New applicant response:', response);
      let errors = response.errors;
      if(errors){
      let msg = '';
      for(let e in errors){
        let er = errors[e];
        if(er && er.length){
          for (let index = 0; index < er.length; index++) {
            const element = er[index];
            msg += element;
          }
        }
      }
      alert(msg);
      } else{
        //this.ea.publish(new ApplicantCreated(this.applicant));
        this.applicant = new Applicant();
        //this.router.navigateToRoute("/applicants");
        alert('Applicants created successfully');
      }
      
    });
  }

  get hasData(){
    return this.applicant.name || 
            this.applicant.familyName  ||
            this.applicant.emailAddress ||
            this.applicant.age ||
            this.applicant.countryOfOrigin ||            
            !this.api.isRequesting;
  }

  
  get canSave(){
    return this.applicant.name && 
            this.applicant.familyName  &&
            this.applicant.emailAddress &&
            this.applicant.age &&
            this.applicant.countryOfOrigin &&            
            !this.api.isRequesting;
  }

  
  canDeactivate() {
    if(this.hasData) {
      let result = confirm('You have unsaved changes. Are you sure you wish to leave?');
      return result;
    }
    return true;
  }
}
