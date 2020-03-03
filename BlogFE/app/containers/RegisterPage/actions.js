/*
 *
 * RegisterPage actions
 *
 */

import {
  REGISTER_REQUEST,
  REGISTER_SUCCESS,
  REGISTER_ERROR,
  SET_USER_NAME,
  SET_PASSWORD,
} from './constants';

export function registerRequest() {
  return {
    type: REGISTER_REQUEST,
  };
}

export function registerSuccess(response) {
  return {
    type: REGISTER_SUCCESS,
    response,
  };
}

export function registerError(error) {
  return {
    type: REGISTER_ERROR,
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
