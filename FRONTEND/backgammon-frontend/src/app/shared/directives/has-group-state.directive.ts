import {
  Directive,
  Input,
  TemplateRef,
  ViewContainerRef,
  effect,
  signal,
} from '@angular/core';

import { GroupUserState } from '../../features/groups/models/enums/group-user-state.type';

@Directive({
  selector: '[appHasGroupState]',
  standalone: true,
})
export class HasGroupStateDirective {
  // aktuális user state
  private currentState = signal<GroupUserState | null>(null);

  // megengedett state-ek
  private allowedStates = signal<GroupUserState[]>([]);

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
  ) {
    effect(() => {
      const state = this.currentState();
      const allowed = this.allowedStates();

      this.viewContainer.clear();

      if (state && allowed.includes(state)) {
        this.viewContainer.createEmbeddedView(this.templateRef);
      }
    });
  }

  // aktuális state
  @Input()
  set appHasGroupStateCurrent(state: GroupUserState | null) {
    this.currentState.set(state);
  }

  // engedélyezett state-ek
  @Input()
  set appHasGroupState(states: GroupUserState[]) {
    this.allowedStates.set(states);
  }
}
