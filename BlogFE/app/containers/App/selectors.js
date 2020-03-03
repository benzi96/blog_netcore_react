import { createSelector } from 'reselect';

const selectRouter = state => state.router;
const selectOidc = state => state.oidc;
const selectGlobal = state => state.global;

const makeSelectLocation = () =>
  createSelector(
    selectRouter,
    routerState => routerState.location,
  );

const makeSelectOidc = () =>
  createSelector(
    selectOidc,
    oidc => oidc,
  );

const makeSelectCategories = () =>
  createSelector(
    selectGlobal,
    state => state.categories,
  );

export { makeSelectLocation, makeSelectOidc, makeSelectCategories };
