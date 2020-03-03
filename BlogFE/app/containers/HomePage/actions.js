/*
 *
 * HomePage actions
 *
 */

import {
  DEFAULT_ACTION,
  GET_LIST_POST_VIEW_REQUEST,
  GET_LIST_POST_VIEW_SUCCESS,
  GET_LIST_POST_VIEW_ERROR,
  RESET_PAGE,
} from './constants';

export function defaultAction() {
  return {
    type: DEFAULT_ACTION,
  };
}

export function getListPostViewRequest(categoryName, page) {
  return {
    type: GET_LIST_POST_VIEW_REQUEST,
    categoryName,
    page,
  };
}

export function getListPostViewSuccess(response) {
  return {
    type: GET_LIST_POST_VIEW_SUCCESS,
    response,
  };
}

export function getListPostViewError(error) {
  return {
    type: GET_LIST_POST_VIEW_ERROR,
    error,
  };
}

export function resetPage() {
  return {
    type: RESET_PAGE,
  };
}
