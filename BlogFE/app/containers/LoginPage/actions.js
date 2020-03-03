/*
 *
 * LoginPage actions
 *
 */

import {
  LOGIN_REQUEST,
  LOGIN_SUCCESS,
  LOGIN_ERROR,
  SET_USER_NAME,
  SET_PASSWORD,
} from './constants';

export function loginRequest() {
  return {
    type: LOGIN_REQUEST,
  };
}

export function loginSuccess(response) {
  return {
    type: LOGIN_SUCCESS,
    response,
  };
}

export function loginError(error) {
  return {
    type: LOGIN_ERROR,
    error,
  };
}

export function setUserName(data) {
  return {
    type: SET_USER_NAME,
    data,
  };
}

export function setPassword(data) {
  return {
    type: SET_PASSWORD,
    data,
  };
}
