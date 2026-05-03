import {
  Directive,
  Input,
  TemplateRef,
  ViewContainerRef,
  effect,
  signal,
} from '@angular/core';
import { GroupRole } from '../../features/groups/models/enums/group-role.type';

@Directive({
  selector: '[appHasGroupRole]',
  standalone: true,
})
export class HasGroupRoleDirective {
  private currentRole = signal<GroupRole | null>(null);
  private allowedRoles = signal<GroupRole[]>([]);

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
  ) {
    // reaktív render
    effect(() => {
      const role = this.currentRole();
      const allowed = this.allowedRoles();

      this.viewContainer.clear();

      if (role && allowed.includes(role)) {
        this.viewContainer.createEmbeddedView(this.templateRef);
      }
    });
  }

  // role input
  @Input()
  set appHasGroupRoleRole(role: GroupRole | null) {
    this.currentRole.set(role);
  }

  // allowed roles
  @Input()
  set appHasGroupRole(roles: GroupRole[]) {
    this.allowedRoles.set(roles);
  }
}
