import { TestBed } from '@angular/core/testing';

import { UserValidatorsService } from './user-validators.service';

describe('UserValidatorsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UserValidatorsService = TestBed.get(UserValidatorsService);
    expect(service).toBeTruthy();
  });
});
