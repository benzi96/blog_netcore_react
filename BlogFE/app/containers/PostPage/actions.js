/*
 *
 * PostPage actions
 *
 */

import {
  LOAD_POST_REQUEST,
  LOAD_POST_SUCCESS,
  LOAD_POST_ERROR,
  RESET_PAGE,
} from './constants';

export function loadPostRequest(id) {
  return {
    type: LOAD_POST_REQUEST,
    id,
  };
}

export function loadPostSuccess(response) {
  return {
    type: LOAD_POST_SUCCESS,
    response,
  };
}

export function loadPostError(error) {
  return {
    type: LOAD_POST_ERROR,
    error,
  };
}

export function resetPage() {
  return {
    type: RESET_PAGE,
  };
}
