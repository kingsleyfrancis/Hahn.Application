import { Applicant } from './applicant';
export class ApplicantUpdated {
  constructor(public applicant: Applicant){}
}

export class ApplicantViewed {
  constructor(public applicant: Applicant){}
}

export class ApplicantCreated{
  constructor(public applicant: Applicant){}
}
