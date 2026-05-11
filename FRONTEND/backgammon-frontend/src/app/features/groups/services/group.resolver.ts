import { inject } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { GroupsApiService } from '../services/groups-api.service';

export const groupResolver = (route: ActivatedRouteSnapshot) => {
  const id = route.paramMap.get('groupId')!;

  return inject(GroupsApiService).getGroupById(id);
};
