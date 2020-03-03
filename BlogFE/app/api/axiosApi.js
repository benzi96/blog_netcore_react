import axios from 'axios';
import config from 'config'; //eslint-disable-line
import { call, select } from 'redux-saga/effects';
import { makeSelectOidc } from 'containers/App/selectors';

const normalRequest = axios.create({
  baseURL: config.baseApi,
});

export function post(url, data) {
  return normalRequest
    .post(url, data)
    .catch(error => {
      throw error;
    })
    .then(response => response);
}

export function* postWithAccessToken(url, data, otherConfig) {
  const oidc = yield select(makeSelectOidc());
  const authorization = { Authorization: `Bearer ${oidc.user.access_token}` };
  const additionConfig = otherConfig || {};
  const option = {
    headers: {
      ...additionConfig,
      ...authorization,
    },
  };

  return normalRequest
    .post(url, data, option)
    .catch(error => {
      throw error;
    })
    .then(response => response);
}

export function request(url, option) {
  return normalRequest
    .get(url, option)
    .catch(error => {
      throw error;
    })
    .then(response => response);
}

export function* get(url) {
  const promise = yield call(request, url, {});
  return promise;
}

export function* getWithAccessToken(url) {
  const oidc = yield select(makeSelectOidc());

  const option = {
    headers: {
      Authorization: `Bearer ${oidc.user.access_token}`,
    },
  };
  const promise = yield call(request, url, option);
  return promise;
}
