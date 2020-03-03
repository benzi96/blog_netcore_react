import {
  GET_CATEGORY_REQUEST,
  GET_CATEGORY_SUCCESS,
  GET_CATEGORY_ERROR,
} from './constants';

export function getCategoryRequest() {
  return {
    type: GET_CATEGORY_REQUEST,
  };
}

export function getCategorySuccess(response) {
  return {
    type: GET_CATEGORY_SUCCESS,
    response,
  };
}

export function getCategoryError(error) {
  return {
    type: GET_CATEGORY_ERROR,
    error,
  };
}
