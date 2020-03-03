/* eslint-disable prettier/prettier */
import { createUserManager } from 'redux-oidc';
import config from 'config'; //eslint-disable-line

const userManagerConfig = {
  client_id: 'spa',
  redirect_uri: `${window.location.protocol}//${window.location.hostname}${window.location.port ? `:${window.location.port}` : ''}/callback`,
  response_type: 'code',
  scope: 'openid profile api1',
  authority: config.authorityUrl,
  silent_redirect_uri: `${window.location.protocol}//${window.location.hostname}${window.location.port ? `:${window.location.port}` : ''}/silent_renew.html`,
  automaticSilentRenew: true,
  filterProtocolClaims: true,
  loadUserInfo: true,
  monitorSession: true,
};

const userManager = createUserManager(userManagerConfig);

export default userManager;
