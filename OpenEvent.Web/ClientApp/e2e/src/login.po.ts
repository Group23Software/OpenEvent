import {browser, by, element} from "protractor";

export class LoginPage
{
  navigateToLogin ()
  {
    return browser.get('/login');
  }

  getLoginComponent ()
  {
    return element(by.css('app-login'));
  }

  getEmailInput ()
  {
    return element(by.id('email'));
  }

  getPasswordInput ()
  {
    return element(by.id('password'));
  }

  getSignUpButton ()
  {
    return element(by.buttonText('Sign up'));
  }

  getCreateAccountDialog ()
  {
    return element(by.css('app-create-account'));
  }

  fillCreateAccount ()
  {
    // element(by.css("input[formControlName=email]")).sendKeys('test@email.co.uk');
    element(by.css('input[formControlName=userName]')).sendKeys('TestUser');
    element(by.css('input[formControlName=firstName]')).sendKeys('Test');
    element(by.css('input[formControlName=lastName]')).sendKeys('User');
    element(by.css('input[formControlName=dOB]')).sendKeys('07/24/2000');
    element(by.css('input[formControlName=phoneNumber]')).sendKeys('0000000000');
    element(by.css('input[formControlName=password]')).sendKeys('Password@1');
  }

  getCompleteSignUpButton ()
  {
    return element(by.id('completeSignUp'));
  }
}
